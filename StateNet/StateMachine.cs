using System;
using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.StateTransitionTable;
using Aptacode.StateNet.Transitions;
using NLog;

namespace Aptacode.StateNet
{
    public class StateMachine
    {
        /// <summary>
        /// Returns the current State
        /// </summary>
        public string State { get; private set; }
        /// <summary>
        /// Returns the last input
        /// </summary>
        public string LastInput { get; private set; }
        public event EventHandler<StateTransitionArgs> OnTransition;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly object _mutex = new object();
        private readonly IStateTransitionTable _stateTransitionTable;

        /// <summary>
        /// Governs the transitions between states based on the inputs it receives
        /// </summary>
        /// <param name="stateCollection"></param>
        /// <param name="inputCollection"></param>
        /// <param name="stateTransitionTable"></param>
        /// <param name="initialState"></param>
        public StateMachine(StateCollection stateCollection, InputCollection inputCollection,
            IStateTransitionTable stateTransitionTable, string initialState)
        {
            _stateTransitionTable = stateTransitionTable;

            _stateTransitionTable.Setup(stateCollection, inputCollection);

            State = initialState;
        }

        /// <summary>
        /// Define a new transition
        /// </summary>
        /// <param name="transition"></param>
        public void Define(Transition transition)
        {
            if (_stateTransitionTable.Get(transition.State, transition.Input) == null)
            {
                _stateTransitionTable.Set(transition);
                Logger.Trace("Registered {0}", transition.ToString());
            }
            else
            {
                Logger.Debug("Duplicate transition, could not register {0}", transition.ToString());
                throw new DuplicateTransitionException(transition);
            }
        }

        /// <summary>
        /// Set a transition to 'Undefined'
        /// </summary>
        /// <param name="transition"></param>
        public void Clear(Transition transition)
        {
            _stateTransitionTable.Clear(transition);
        }

        /// <summary>
        /// Apply the transition which relates to the given input on the current state
        /// </summary>
        /// <param name="input"></param>
        public void Apply(string input)
        {
            lock (_mutex)
            {
                var transition = GetValidTransition(State, input);
                var nextState = transition.Apply();
                LastInput = input;
                UpdateState(nextState);
            }
        }

        private Transition GetValidTransition(string state, string input)
        {
            var transition = _stateTransitionTable.Get(state, input);

            if (transition == null) throw new UndefinedTransitionException(State, input);

            var validTransition = transition as ValidTransition;
            if (validTransition == null) throw new InvalidTransitionException(State, input);

            return transition;
        }

        private void UpdateState(string nextState)
        {
            var oldState = State;
            State = nextState;
            OnTransition?.Invoke(this, new StateTransitionArgs(oldState, LastInput, nextState));
        }
    }
}