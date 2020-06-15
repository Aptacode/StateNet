using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.StateNet.Engine.Connections;
using Aptacode.StateNet.Engine.Events;
using Aptacode.StateNet.Engine.History;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine
{
    public class StateNetEngine : IStateNetEngine
    {
        private readonly Dictionary<State, List<Action>> _callbackDictionary;
        private readonly ConnectionChooser _connectionChooser;
        private readonly ConcurrentQueue<Input> _inputQueue;
        private readonly IStateNetwork _stateNetwork;
        private readonly CancellationToken cancellationToken;
        private readonly CancellationTokenSource cancellationTokenSource;

        public StateNetEngine(IRandomNumberGenerator randomNumberGenerator, IStateNetwork stateNetwork)
        {
            History = new EngineHistory();
            _stateNetwork = stateNetwork;
            _connectionChooser = new ConnectionChooser(randomNumberGenerator, History);
            _callbackDictionary = new Dictionary<State, List<Action>>();
            _inputQueue = new ConcurrentQueue<Input>();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public State CurrentState { get; private set; }

        public IEngineHistory History { get; }

        public bool IsRunning() => CurrentState?.IsEnd() == false;

        public event EventHandler<EngineStartedEventArgs> OnStarted;

        public event EventHandler<EngineFinishedEventArgs> OnFinished;

        public event EventHandler<EngineTransitionEventArgs> OnTransition;

        public void Subscribe(State state, Action callback)
        {
            if (!_callbackDictionary.TryGetValue(state, out var actions))
            {
                actions = new List<Action>();
                _callbackDictionary.Add(state, actions);
            }

            actions.Add(callback);
        }

        public void Unsubscribe(State state, Action callback)
        {
            if (_callbackDictionary.TryGetValue(state, out var actions))
            {
                actions.Remove(callback);
            }
        }

        public void Start()
        {
            if (!_stateNetwork.IsValid())
            {
                return;
            }

            CurrentState = _stateNetwork.StartState;

            OnStarted?.Invoke(this, new EngineStartedEventArgs(CurrentState));

            History.SetStart(CurrentState);

            new TaskFactory().StartNew(async () =>
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    NextTransition();
                    await Task.Delay(1, cancellationToken).ConfigureAwait(false);
                }
            }, cancellationToken).ConfigureAwait(false);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public bool Apply(string inputName)
        {
            var input = _stateNetwork.GetInput(inputName);

            if (input == null)
            {
                return false;
            }

            _inputQueue.Enqueue(input);

            return true;
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
        }

        private void NotifySubscribers(State state)
        {
            if (_callbackDictionary.ContainsKey(state))
            {
                _callbackDictionary[state]?.ForEach(callback =>
                {
                    new TaskFactory().StartNew(() => callback?.Invoke(), cancellationToken)
                        .ConfigureAwait(false);
                });
            }
        }

        private void NextTransition()
        {
            if (!_inputQueue.TryDequeue(out var input))
            {
                return;
            }

            var lastState = CurrentState;
            var nextState = GetNextState(lastState, input);
            if (nextState == null)
            {
                return;
            }


            History.Log(lastState, input, nextState);

            new TaskFactory()
                .StartNew(
                    () => OnTransition?.Invoke(this, new EngineTransitionEventArgs(lastState, input, nextState)),
                    cancellationToken)
                .ConfigureAwait(false);

            CurrentState = nextState;

            if (CurrentState.IsEnd())
            {
                OnFinished?.Invoke(this, new EngineFinishedEventArgs(CurrentState));
            }
            else
            {
                NotifySubscribers(CurrentState);
            }
        }

        private State GetNextState(State state, string input)
        {
            var connections = _stateNetwork.GetConnections(state, input);
            return _connectionChooser.Choose(connections).Target;
        }
    }
}