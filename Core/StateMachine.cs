using Aptacode.StateNet.Core.StateTransitionTable;
using Aptacode.StateNet.Core.Transitions;
using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;

namespace Aptacode.StateNet.Core
{
    public class StateMachine
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<StateTransitionArgs> OnTransition;
        private static readonly Object mutex = new Object();

        private readonly IStateTransitionTable StateTransitionTable;
        private readonly StateCollection StateCollection;
        private readonly InputCollection InputCollection;

        public string State { get; private set; }
        public string LastInput { get; private set; }

        public StateMachine(StateCollection stateCollection, InputCollection inputCollection, IStateTransitionTable stateTransitionTable, string initialState)
        {
            StateCollection = stateCollection;
            InputCollection = inputCollection;
            StateTransitionTable = stateTransitionTable;

            StateTransitionTable.Setup(stateCollection, inputCollection);

            State = initialState;
        }

        public void Define(Transition transition)
        {
            if (StateTransitionTable.Get(transition.State, transition.Input) == null)
            {
                StateTransitionTable.Set(transition);
                Logger.Trace("Registered {0}", transition.ToString());
            }
            else
            {
                Logger.Debug("Duplicate transition, could not register {0}", transition.ToString());
                throw new DuplicateTransitionException(transition);
            }
        }

        public void Clear(Transition transition)
        {
            StateTransitionTable.Clear(transition);
        }

        public void Apply(string input)
        {
            lock (mutex)
            {
                var transition = GetValidTransition(State, input);
                var nextState = transition.Apply();
                LastInput = input;
                UpdateState(nextState);
            }
        }

        private Transition GetValidTransition(string state, string input)
        {
            var transition = StateTransitionTable.Get(state, input);

            if (transition == null)
            {
                throw new UndefinedTransitionException(State, input);
            }

            var validTransition = transition as ValidTransition;
            if (validTransition == null)
            {
                throw new InvalidTransitionException(State, input);
            }

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
