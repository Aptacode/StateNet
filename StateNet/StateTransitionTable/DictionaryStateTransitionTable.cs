using System.Collections.Generic;
using Aptacode.StateNet.Transitions;

namespace Aptacode.StateNet.StateTransitionTable
{
    public class DictionaryStateTransitionTable : IStateTransitionTable
    {
        private readonly Dictionary<string, Dictionary<string, Transition>> _transitions;

        /// <summary>
        ///     A StateTransitionTable based on a dictionary
        /// </summary>
        public DictionaryStateTransitionTable()
        {
            _transitions = new Dictionary<string, Dictionary<string, Transition>>();
        }

        /// <summary>
        ///     Set each possible transition to null
        /// </summary>
        /// <param name="stateCollection"></param>
        /// <param name="inputCollection"></param>
        public void Setup(StateCollection stateCollection, InputCollection inputCollection)
        {
            foreach (var state in stateCollection.GetStates())
            {
                var stateDictionary = new Dictionary<string, Transition>();

                foreach (var input in inputCollection.GetInputs())
                {
                    stateDictionary.Add(input, null);
                }

                _transitions.Add(state, stateDictionary);
            }
        }

        /// <summary>
        ///     Define a transition
        /// </summary>
        /// <param name="transition"></param>
        public void Set(Transition transition)
        {
            _transitions[transition.State][transition.Input] = transition;
        }

        /// <summary>
        ///     Return the transition for the given state and input
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public Transition Get(string state, string input)
        {
            return _transitions[state][input];
        }

        /// <summary>
        ///     Remove the transition setting the transition at its State and Input to null
        /// </summary>
        /// <param name="transition"></param>
        public void Clear(Transition transition)
        {
            _transitions[transition.State][transition.Input] = null;
        }
    }
}