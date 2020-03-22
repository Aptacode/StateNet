using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aptacode.StateNet.Events;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{
    public class Engine : IEngine
    {
        private readonly Dictionary<State, List<Action>> _callbackDictionary;
        private readonly EngineLog _engineLog;
        private readonly ConcurrentQueue<Input> _inputQueue;
        private readonly INetwork _network;
        private readonly StateChooser _stateChooser;
        private readonly CancellationToken cancellationToken;
        private readonly CancellationTokenSource cancellationTokenSource;

        public Engine(IRandomNumberGenerator randomNumberGenerator, INetwork network)
        {
            _network = network;
            _engineLog = new EngineLog();
            _stateChooser = new StateChooser(randomNumberGenerator, _engineLog);
            _callbackDictionary = new Dictionary<State, List<Action>>();
            _inputQueue = new ConcurrentQueue<Input>();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public State CurrentState { get; private set; }

        public event EngineEvent OnFinished;

        public event EngineEvent OnStarted;

        public event TransitionEvent OnTransition;

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

        public EngineLog GetLog()
        {
            return _engineLog;
        }

        public void Start()
        {
            if (!_network.IsValid())
            {
                return;
            }

            OnStarted?.Invoke(this);
            CurrentState = _network.StartState;

            _engineLog.Add(Input.Empty, CurrentState);

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
            var input = _network.GetInput(inputName, false);

            if (Input.Empty.Equals(input))
            {
                return false;
            }

            _inputQueue.Enqueue(input);

            return true;
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

            var nextState = GetNextState(CurrentState, input);
            if (nextState == null)
            {
                return;
            }

            _engineLog.Add(input, nextState);

            new TaskFactory().StartNew(() => OnTransition?.Invoke(this, CurrentState, input, nextState), cancellationToken).ConfigureAwait(false);

            CurrentState = nextState;

            if (CurrentState.IsEnd())
            {
                OnFinished?.Invoke(this);
            }
            else
            {
                NotifySubscribers(CurrentState);
            }
        }

        private State GetNextState(State state, string input)
        {
            var stateName = _stateChooser.Choose(_network[state, input]);
            return _network[stateName];
        }
    }
}