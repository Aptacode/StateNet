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
        private readonly List<(Input, State)> _history;
        private readonly ConcurrentQueue<Input> _inputQueue;
        private readonly INetwork _network;
        private readonly StateChooser _stateChooser;
        private readonly CancellationToken cancellationToken;
        private readonly CancellationTokenSource cancellationTokenSource;

        public Engine(IRandomNumberGenerator randomNumberGenerator, INetwork network)
        {
            _network = network;
            _history = new List<(Input, State)>();
            _stateChooser = new StateChooser(randomNumberGenerator, _history);
            _callbackDictionary = new Dictionary<State, List<Action>>();
            _inputQueue = new ConcurrentQueue<Input>();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public State CurrentState { get; private set; }

        public event EngineEvent OnFinished;

        public event EngineEvent OnStarted;

        public event StateEvent OnTransition;

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

        public List<(Input, State)> GetHistory()
        {
            return _history;
        }

        public void Start()
        {
            if (!_network.IsValid())
            {
                return;
            }

            SubscribeToNodesVisited();
            SubscribeToEndNodes();
            OnStarted?.Invoke(this);
            CurrentState = _network.StartState;
            _history.Add((Input.Empty, CurrentState));

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

            if (input.Equals(Input.Empty))
            {
                return false;
            }

            _inputQueue.Enqueue(input);

            return true;
        }

        private void SubscribeToEndNodes()
        {
            foreach (var node in _network.GetEndStates())
            {
                node.OnVisited += delegate { OnFinished?.Invoke(this); };
            }
        }

        private void SubscribeToNodesVisited()
        {
            foreach (var node in _network.GetStates())
            {
                node.OnVisited += NotifySubscribers;
            }
        }

        private void NotifySubscribers(State state)
        {
            new TaskFactory().StartNew(() => OnTransition?.Invoke(state), cancellationToken).ConfigureAwait(false);

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

            _history.Add((input, CurrentState));

            CurrentState.UpdateChoosers();

            var nextState = GetNextState(CurrentState, input);
            if (nextState == null)
            {
                return;
            }

            CurrentState.Exit();
            CurrentState = nextState;
            CurrentState.Visit();
        }

        private State GetNextState(State state, string actionName)
        {
            var stateName = _stateChooser.Choose(_network[state, actionName]);
            return _network[stateName];
        }
    }
}