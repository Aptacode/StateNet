using Aptacode.StateNet.Events;
using Aptacode.StateNet.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aptacode.StateNet
{
    public class Engine : IEngine
    {
        private readonly INetwork _network;
        private readonly List<State> _history;
        private readonly StateChooser _stateChooser;
        private readonly ConcurrentQueue<string> _inputQueue;
        private readonly Dictionary<State, List<Action>> _callbackDictionary;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly CancellationToken cancellationToken;
        public State CurrentState { get; private set; }

        public Engine(INetwork network)
        {
            _network = network;
            _history = new List<State>();
            _stateChooser = new StateChooser(_history);
            _callbackDictionary = new Dictionary<State, List<Action>>();
            _inputQueue = new ConcurrentQueue<string>();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
        }

        public event EngineEvent OnFinished;

        public event EngineEvent OnStarted;

        public event StateEvent OnTransition;

        private void SubscribeToEndNodes()
        {
            foreach (var node in _network.GetEndStates())
            {
                node.OnVisited += (s) =>
                {
                    OnFinished?.Invoke(this);
                };
            }
        }

        private void SubscribeToNodesVisited()
        {
            foreach (var node in _network.GetAll())
            {
                node.OnVisited += (sender) =>
                {
                    _history.Add(sender);

                    NotifySubscribers(sender);
                };
            }
        }

        private void NotifySubscribers(State state)
        {
            new TaskFactory().StartNew(() =>
            {
                OnTransition?.Invoke(state);

            }).ConfigureAwait(false);

            if (_callbackDictionary.ContainsKey(state))
            {
                _callbackDictionary[state]?.ForEach(callback =>
                {
                    new TaskFactory().StartNew(() =>
                    {
                        callback?.Invoke();
                    }).ConfigureAwait(false);
                });
            }
        }

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

        public List<State> GetHistory() => _history;

        public void Start()
        {
            if (_network.IsValid())
            {
                SubscribeToNodesVisited();
                SubscribeToEndNodes();
                OnStarted?.Invoke(this);
                CurrentState = _network.StartState;
                _history.Add(CurrentState);

                new TaskFactory().StartNew(async () =>
                {
                    while (true)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        NextTransition();
                        await Task.Delay(1).ConfigureAwait(false);
                    }
                }, cancellationToken).ConfigureAwait(false);
            }
        }

        public void Stop() => cancellationTokenSource.Cancel();

        public void Apply(string actionName) => _inputQueue.Enqueue(actionName);

        private void NextTransition()
        {
            if (_inputQueue.TryDequeue(out var actionName))
            {
                CurrentState.UpdateChoosers();

                var nextState = GetNextState(CurrentState, actionName);
                if (nextState != null)
                {
                    CurrentState.Exit();
                    CurrentState = nextState;
                    CurrentState.Visit();
                }
            }
        }
        private State GetNextState(State state, string actionName) => _stateChooser.Choose(_network[state, actionName]);
    }
}