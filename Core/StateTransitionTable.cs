using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;
using System.Linq;

namespace Aptacode.StateNet.Core
{
    public class StateTransitionTable<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        private readonly Transition<States, Actions>[][] transitions;

        public StateTransitionTable()
        {
            transitions = new Transition<States, Actions>[Enum.GetValues(typeof(States)).Length][];
            CreateTransitionTable();
        }
        private void CreateTransitionTable()
        {
            for (int i = 0; i < transitions.Count(); i++)
            {
                transitions[i] = new Transition<States, Actions>[Enum.GetValues(typeof(Actions)).Length];
                for (int j = 0; j < transitions[i].Count(); j++)
                {
                    transitions[i][j] = null;
                }
            }
        }

        public void Set(Transition<States, Actions> transition)
        {
            transitions[Convert.ToInt32(transition.State)][Convert.ToInt32(transition.Action)] = transition;
        }

        public Transition<States, Actions> Get(States state, Actions action)
        {
            return transitions[Convert.ToInt32(state)][Convert.ToInt32(action)];
        }

        internal void Clear(Transition<States, Actions> transition)
        {
            transitions[Convert.ToInt32(transition.State)][Convert.ToInt32(transition.Action)] = null;
        }
    }
}
