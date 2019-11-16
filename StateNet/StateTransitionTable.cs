using Aptacode.StateNet.Transitions;
using System.Collections.Concurrent;
using System.Linq;

namespace Aptacode.StateNet
{
    public class StateTransitionTable
    {
        readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Transition>> _transitions;

        public StateTransitionTable(StateCollection states, InputCollection inputs) => _transitions =
            CreateEmptyTransitionTable(states, inputs);

        ConcurrentDictionary<string, ConcurrentDictionary<string, Transition>> CreateEmptyTransitionTable(StateCollection states,
                                                                                                          InputCollection inputs)
        {
            var transitionDictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, Transition>>();

            foreach(var state in states)
            {
                transitionDictionary.TryAdd(state, CreateEmptyInputDictionary(inputs));
            }

            return transitionDictionary;
        }

        ConcurrentDictionary<string, Transition> CreateEmptyInputDictionary(InputCollection inputs)
        {
            var stateDictionary = new ConcurrentDictionary<string, Transition>();

            foreach(var input in inputs)
            {
                stateDictionary.TryAdd(input, null);
            }

            return stateDictionary;
        }

        /// <summary>
        /// Define a transition
        /// </summary>
        /// <param name="transition"></param>
        public bool Set(Transition newTransition)
        {
            if(newTransition == null)
            {
                return false;
            }

            if(_transitions.TryGetValue(newTransition.State, out var inputDictionary))
            {
                inputDictionary.TryGetValue(newTransition.Input, out var oldTransition);
                if(inputDictionary.TryUpdate(newTransition.Input, newTransition, oldTransition))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the transition for the given state and input
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public Transition Get(string state, string input)
        {
            Transition transition = null;
            if(_transitions.TryGetValue(state, out var inputDictionary))
            {
                inputDictionary.TryGetValue(input, out transition);
            }

            return transition;
        }

        /// <summary>
        /// Remove the transition setting the transition at its State and Input to null
        /// </summary>
        /// <param name="transition"></param>
        public bool Clear(Transition oldTransition)
        {
            if(_transitions.TryGetValue(oldTransition.State, out var inputDictionary))
            {
                if(inputDictionary.TryUpdate(oldTransition.Input, null, oldTransition))
                {
                    return true;
                }
            }

            return false;
        }
    }
}