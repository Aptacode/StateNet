using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Core.StateTransitionTable
{
    public class DictionaryStateTransitionTable : IStateTransitionTable
    {
        private readonly Dictionary<string, Dictionary<string, Transition>> Transitions;

        public DictionaryStateTransitionTable()
        {
            Transitions = new Dictionary<string, Dictionary<string, Transition>>();

        }
        public void Setup(StateCollection stateCollection, InputCollection inputCollection)
        {
            foreach (string state in stateCollection.GetStates())
            {
                var stateDictionary = new Dictionary<string, Transition>();

                foreach (string input in inputCollection.GetInputs())
                {
                    stateDictionary.Add(input, null);
                }
                Transitions.Add(state, stateDictionary);
            }
        }

        public void Set(Transition transition)
        {
            Transitions[transition.State][transition.Input] = transition;
        }

        public Transition Get(string state, string input)
        {
            return Transitions[state][input];
        }

        public void Clear(Transition transition)
        {
            Transitions[transition.State][transition.Input] = null;
        }


    }
}
