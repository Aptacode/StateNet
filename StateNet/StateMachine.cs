using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.Transitions;
using Aptacode.StateNet.TransitionTables;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.StateNet
{
    public class StateMachine : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, List<Action>> _callbackDictionary;
        private bool _isRunning;
        private readonly StateTransitionTable _stateTransitionTable;
        private readonly ConcurrentQueue<string> inputQueue;

        /// <summary>
        /// Governs the transitions between states based on the inputs it receives
        /// </summary>
        public StateMachine(StateTransitionTable stateTransitionTable, string initialState)
        {
            _stateTransitionTable = stateTransitionTable;
            inputQueue = new ConcurrentQueue<string>();
            SetInitialState(initialState);

            _callbackDictionary = new Dictionary<string, List<Action>>();
        }

        public event EventHandler<InvalidStateTransitionArgs> OnInvalidTransition;

        public event EventHandler<StateTransitionArgs> OnTransition;

        private void NextTransition()
        {
            if(inputQueue.TryDequeue(out var input))
            {
                try
                {
                    var transition = _stateTransitionTable.Get(State, input);
                    var nextState = transition.Apply();
                    LastInput = input;
                    UpdateState(nextState);
                } catch
                {
                    Logger.Error("Queued transition was invalid");
                    OnInvalidTransition?.Invoke(this, new InvalidStateTransitionArgs(State, input));
                }
            }
        }

        private void SetInitialState(string initialState)
        {
            if(_stateTransitionTable.States.Contains(initialState))
            {
                State = initialState;
            } else
            {
                State = _stateTransitionTable.States.First();
            }
        }

        private void UpdateState(string nextState)
        {
            var oldState = State;
            State = nextState;
            OnTransition?.Invoke(this, new StateTransitionArgs(oldState, LastInput, nextState));
            _callbackDictionary[nextState].ForEach(callback => callback?.Invoke());
        }

        /// <summary>
        /// Apply the transition which relates to the given input on the current state
        /// </summary>
        /// <param name="input"></param>
        public void Apply(string input) => inputQueue.Enqueue(input) ;

        /// <summary>
        /// Set a transition to 'Undefined'
        /// </summary>
        /// <param name="transition"></param>
        public void Clear(BaseTransition transition) => _stateTransitionTable.Clear(transition) ;

        /// <summary>
        /// Define a new transition
        /// </summary>
        /// <param name="transition"></param>
        public void Define(BaseTransition transition)
        {
            if(!_stateTransitionTable.Set(transition))
            {
                throw new InvalidTransitionException(transition.Origin, transition.Input);
            }
        }

        public void Dispose() => Stop() ;

        public void Start() => new TaskFactory().StartNew(async() =>
        {
            _isRunning = true;

            while(_isRunning)
            {
                NextTransition();
                await Task.Delay(1).ConfigureAwait(false);
            }
        }) ;

        public void Stop() => _isRunning = false ;

        public void Subscribe(string state, Action callback)
        {
            if(!_callbackDictionary.TryGetValue(state, out var listeners))
            {
                listeners = new List<Action> { callback };
                _callbackDictionary.Add(state, listeners);
            }

            listeners.Add(callback);
        }

        public void UnSubscribe(string state, Action callback)
        {
            if(_callbackDictionary.TryGetValue(state, out var listeners))
            {
                listeners.Remove(callback);
            }
        }

        /// <summary>
        /// Returns the last input
        /// </summary>
        public string LastInput { get; private set; }

        /// <summary>
        /// Returns the current State
        /// </summary>
        public string State { get; private set; }
    }
}