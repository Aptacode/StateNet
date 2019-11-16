using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.Transitions;
using NLog;
using System;
using System.Linq;

namespace Aptacode.StateNet
{
    public class StateMachine
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        readonly StateTransitionTable _stateTransitionTable;
        readonly StateCollection _states;
        readonly InputCollection _inputs;

        /// <summary>
        /// Governs the transitions between states based on the inputs it receives
        /// </summary>
        public StateMachine(StateCollection states, InputCollection inputs, string initialState)
        {
            _states = states;
            _inputs = inputs;
            _stateTransitionTable = new StateTransitionTable(_states, _inputs);
            SetInitialState(initialState);
        }

        void SetInitialState(string initialState)
        {
            if(_states.Contains(initialState))
            {
                State = initialState;
            } else
            {
                State = _states.First();
            }
        }

        /// <summary>
        /// Returns the current State
        /// </summary>
        public string State { get; private set; }

        /// <summary>
        /// Returns the last input
        /// </summary>
        public string LastInput { get; private set; }

        public event EventHandler<StateTransitionArgs> OnTransition;

        /// <summary>
        /// Define a new transition
        /// </summary>
        /// <param name="transition"></param>
        public void Define(Transition transition)
        {
            if(_stateTransitionTable.Get(transition.State, transition.Input) == null)
            {
                _stateTransitionTable.Set(transition);
                Logger.Trace("Registered {0}", transition.ToString());
            } else
            {
                Logger.Debug("Duplicate transition, could not register {0}", transition.ToString());
                throw new DuplicateTransitionException(transition);
            }
        }

        /// <summary>
        /// Set a transition to 'Undefined'
        /// </summary>
        /// <param name="transition"></param>
        public void Clear(Transition transition) => _stateTransitionTable.Clear(transition) ;

        /// <summary>
        /// Apply the transition which relates to the given input on the current state
        /// </summary>
        /// <param name="input"></param>
        public void Apply(string input)
        {
            var transition = GetValidTransition(State, input);
            var nextState = transition.Apply();
            LastInput = input;
            UpdateState(nextState);
        }

        ValidTransition GetValidTransition(string state, string input)
        {
            var transition = _stateTransitionTable.Get(state, input);

            if(transition == null)
            {
                throw new UndefinedTransitionException(State, input);
            }

            if(transition is ValidTransition validTransition)
            {
                return validTransition;
            }

            throw new InvalidTransitionException(State, input);
        }

        void UpdateState(string nextState)
        {
            var oldState = State;
            State = nextState;
            OnTransition?.Invoke(this, new StateTransitionArgs(oldState, LastInput, nextState));
        }
    }
}