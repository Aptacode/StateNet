using System;
using System.Collections.Generic;
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
        private readonly IStateNetwork _stateNetwork;

        public StateNetEngine(IRandomNumberGenerator randomNumberGenerator, IStateNetwork stateNetwork)
        {
            History = new EngineHistory();
            _stateNetwork = stateNetwork;
            _connectionChooser = new ConnectionChooser(randomNumberGenerator, History);
            _callbackDictionary = new Dictionary<State, List<Action>>();
        }

        public State CurrentState { get; private set; }

        public IEngineHistory History { get; }

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
        }

        public bool Apply(string inputName)
        {
            var input = _stateNetwork.GetInput(inputName);

            if (input == null)
            {
                return false;
            }

            var lastState = CurrentState;
            var nextState = GetNextState(lastState, input);
            if (nextState == null)
            {
                return false;
            }

            History.Log(lastState, input, nextState);

            OnTransition?.Invoke(this, new EngineTransitionEventArgs(lastState, input, nextState));

            CurrentState = nextState;

            if (CurrentState.IsEnd())
            {
                OnFinished?.Invoke(this, new EngineFinishedEventArgs(CurrentState));
            }
            else
            {
                NotifySubscribers(CurrentState);
            }

            return true;
        }

        public bool IsRunning() => CurrentState?.IsEnd() == false;

        private void NotifySubscribers(State state)
        {
            if (_callbackDictionary.ContainsKey(state))
            {
                _callbackDictionary[state]?.ForEach(callback => { callback?.Invoke(); });
            }
        }

        private State GetNextState(State state, string input)
        {
            var connections = _stateNetwork.GetConnections(state, input);
            return _connectionChooser.Choose(connections).Target;
        }
    }
}