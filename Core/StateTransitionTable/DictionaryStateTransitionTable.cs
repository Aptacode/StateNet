using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Core.StateTransitionTable
{
    public class DictionaryStateTransitionTable<States, Actions> : IStateTransitionTable<States,Actions> where States : struct, Enum where Actions : struct, Enum
    {
        private readonly Dictionary<States, Dictionary<Actions, Transition<States, Actions>>> transitions;

        public DictionaryStateTransitionTable()
        {
            transitions = new Dictionary<States, Dictionary<Actions, Transition<States, Actions>>>();

            CreateTransitionTable();
        }
        private void CreateTransitionTable()
        {
            foreach (States state in (States[])Enum.GetValues(typeof(States)))
            {
                var stateDictionary = new Dictionary<Actions, Transition<States, Actions>>();

                foreach (Actions action in (Actions[])Enum.GetValues(typeof(Actions)))
                {
                    stateDictionary.Add(action, null);
                }
                transitions.Add(state, stateDictionary);
            }
        }

        public void Set(Transition<States, Actions> transition)
        {
            transitions[transition.State][transition.Action] = transition;
        }

        public Transition<States, Actions> Get(States state, Actions action)
        {
            return transitions[state][action];
        }

        public void Clear(Transition<States, Actions> transition)
        {
            transitions[transition.State][transition.Action] = null;
        }
    }
}
