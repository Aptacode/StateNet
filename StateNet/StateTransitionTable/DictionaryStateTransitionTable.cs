using System.Collections.Generic;
using Aptacode.StateNet.Transitions;

namespace Aptacode.StateNet.StateTransitionTable
{
    public class DictionaryStateTransitionTable : IStateTransitionTable
    {
        private readonly Dictionary<string, Dictionary<string, Transition>> _transitions;

        public DictionaryStateTransitionTable()
        {
            _transitions = new Dictionary<string, Dictionary<string, Transition>>();
        }

        public void Setup(StateCollection stateCollection, InputCollection inputCollection)
        {
            foreach (var state in stateCollection.GetStates())
            {
                var stateDictionary = new Dictionary<string, Transition>();

                foreach (var input in inputCollection.GetInputs()) stateDictionary.Add(input, null);
                _transitions.Add(state, stateDictionary);
            }
        }

        public void Set(Transition transition)
        {
            _transitions[transition.State][transition.Input] = transition;
        }

        public Transition Get(string state, string input)
        {
            return _transitions[state][input];
        }

        public void Clear(Transition transition)
        {
            _transitions[transition.State][transition.Input] = null;
        }
    }
}