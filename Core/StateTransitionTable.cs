using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;
using System.Linq;

namespace Aptacode.StateNet.Core
{
    public class StateTransitionTable<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        private readonly Transition<States, Actions>[][] transitions;
        private readonly int lowestStateValue, highestStateValue;
        private readonly int lowestActionValue, highestActionValue;
        public StateTransitionTable()
        {
            lowestStateValue = Convert.ToInt32(Enum.GetValues(typeof(States)).Cast<States>().Min());
            highestStateValue = Convert.ToInt32(Enum.GetValues(typeof(States)).Cast<States>().Max());
            lowestActionValue = Convert.ToInt32(Enum.GetValues(typeof(Actions)).Cast<Actions>().Min());
            highestActionValue = Convert.ToInt32(Enum.GetValues(typeof(Actions)).Cast<Actions>().Max());

            transitions = new Transition<States, Actions>[1 + highestStateValue - lowestStateValue][];
            CreateTransitionTable();
        }
        private void CreateTransitionTable()
        {

            for (int i = 0; i < transitions.Count(); i++)
            {
                transitions[i] = new Transition<States, Actions>[1 + highestActionValue - lowestActionValue];
                for (int j = 0; j < transitions[i].Count(); j++)
                {
                    transitions[i][j] = null;
                }
            }
        }

        public void Set(Transition<States, Actions> transition)
        {
            transitions[Convert.ToInt32(transition.State) - lowestStateValue][Convert.ToInt32(transition.Action) - lowestActionValue] = transition;
        }

        public Transition<States, Actions> Get(States state, Actions action)
        {
            return transitions[Convert.ToInt32(state) - lowestStateValue][Convert.ToInt32(action) - lowestActionValue];
        }

        internal void Clear(Transition<States, Actions> transition)
        {
            transitions[Convert.ToInt32(transition.State) - lowestStateValue][Convert.ToInt32(transition.Action) - lowestActionValue] = null;
        }
    }
}
