using Aptacode.StateNet.Core.Transitions;
using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;

namespace Aptacode.StateNet.Core
{
    public class StateMachine<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<StateTransitionArgs<States, Actions>> OnTransition;
        private static readonly Object mutex = new Object();
        private readonly StateTransitionTable<States, Actions> stateTransitionTable;

        public States State { get; private set; }
        public Actions LastAction { get; private set; }

        public StateMachine(States initialState)
        {
            State = initialState;
            stateTransitionTable = new StateTransitionTable<States, Actions>();
        }

        public void Define(Transition<States, Actions> transition)
        {
            if (stateTransitionTable.Get(transition.State, transition.Action) == null)
            {
                stateTransitionTable.Set(transition);
                Logger.Trace("Registered {0}", transition.ToString());
            }
            else
            {
                Logger.Debug("Duplicate transition, could not register {0}", transition.ToString());
                throw new DuplicateTransitionException<States, Actions>(transition);
            }
        }

        public void Clear(Transition<States, Actions> transition)
        {
            stateTransitionTable.Clear(transition);
        }

        public void Apply(Actions action)
        {
            lock (mutex)
            {
                var transition = GetValidTransition(State, action);
                var nextState = transition.Apply();
                LastAction = action;
                UpdateState(nextState);
            }
        }

        private Transition<States, Actions> GetValidTransition(States state, Actions action)
        {
            var transition = stateTransitionTable.Get(state, action);

            if (transition == null)
            {
                throw new UndefinedTransitionException<States, Actions>(State, action);
            }

            var validTransition = transition as ValidTransition<States, Actions>;
            if (validTransition == null)
            {
                throw new InvalidTransitionException<States, Actions>(State, action);
            }

            return transition;
        }

        private void UpdateState(States nextState)
        {
            var oldState = State;
            State = nextState;
            OnTransition?.Invoke(this, new StateTransitionArgs<States, Actions>(oldState, LastAction, nextState));
        }

    }
}
