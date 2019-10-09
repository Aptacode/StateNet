using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode_StateMachine.StateNet.Core
{
    public class StateMachine<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public Actions LastAction { get; private set; }
        public States State { get; private set; }
        public event EventHandler<StateTransitionArgs<States, Actions>> OnTransition;
        private List<Transition<States, Actions>> transitions;
        public StateMachine(States initialState)
        {
            State = initialState;
            transitions = new List<Transition<States, Actions>>();
        }

        public void Define(Transition<States, Actions> transition)
        {
            //Add a transition for the given state and action if one is not already defined
            if (GetTransition(transition.State, transition.Action) == null)
            {
                transitions.Add(transition);
                Logger.Trace("Registered {0}", transition.ToString());
            }
            else
            {
                Logger.Debug("Duplicate transition, could not register {0}", transition.ToString());
                throw new DuplicateTransitionException<States, Actions>(transition);
            }
        }

        public void Apply(Actions action)
        {
            //Get the transition defined for the current state and applied action
            var transition = GetTransition(action);

            if (transition == null)
                throw new UndefinedTransitionException<States, Actions>(State, action);

            var validTransition = transition as ValidTransition<States, Actions>;
            if (validTransition == null)
                throw new InvalidTransitionException<States, Actions>(State, action);

            LastAction = action;

            //Get the next state by applying the transition
            var nextState = transition.Apply();

            var oldState = State;
            State = nextState;
            OnTransition?.Invoke(this, new StateTransitionArgs<States, Actions>(oldState, LastAction, nextState));
        }


        /// <summary>
        /// Get the Transition associated with the specified Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns>Returns the state transition associated with the current state and a given Action. Returns Null if the transition does not exist</returns>
        private Transition<States, Actions> GetTransition(Actions action)
        {
            return transitions.Where(p => p.State.Equals(State) && p.Action.Equals(action)).FirstOrDefault();
        }

        /// <summary>
        /// Get the Transition associated with the specified State and Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns>Returns the state transition associated with the given state and Action. Returns Null if the transition does not exist</returns>
        private Transition<States, Actions> GetTransition(States state, Actions action)
        {
            return transitions.Where(p => p.State.Equals(state) && p.Action.Equals(action)).FirstOrDefault();
        }
    }
}
