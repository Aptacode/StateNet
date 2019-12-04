using Aptacode.StateNet.TableMachine.Inputs;
using Aptacode.StateNet.TableMachine.States;
using Aptacode.StateNet.TableMachine.Transitions;
using System.Collections.Concurrent;

namespace Aptacode.StateNet.TableMachine.Tables
{
    public abstract class TransitionTable
    {
        protected readonly ConcurrentDictionary<State, ConcurrentDictionary<Input, BaseTransition>> _transitions;

        protected TransitionTable(StateCollection states, InputCollection inputs)
        {
            States = states;
            Inputs = inputs;
            _transitions = CreateEmptyTransitionTable();
        }

        private ConcurrentDictionary<Input, BaseTransition> CreateEmptyInputDictionary(State state)
        {
            var stateDictionary = new ConcurrentDictionary<Input, BaseTransition>();

            foreach(var input in Inputs)
            {
                stateDictionary.TryAdd(input, new InvalidTransition(state, input, "Undefined"));
            }

            return stateDictionary;
        }

        private ConcurrentDictionary<State, ConcurrentDictionary<Input, BaseTransition>> CreateEmptyTransitionTable()
        {
            var transitionDictionary = new ConcurrentDictionary<State, ConcurrentDictionary<Input, BaseTransition>>();

            foreach(var state in States)
            {
                transitionDictionary.TryAdd(state, CreateEmptyInputDictionary(state));
            }

            return transitionDictionary;
        }

        /// <summary>
        /// Define a transition
        /// </summary>
        /// <param name="transition"></param>
        protected bool Set(BaseTransition newTransition)
        {
            if(newTransition == null)
            {
                return false;
            }

            if(_transitions.TryGetValue(newTransition.Origin, out var inputDictionary))
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
        /// Remove the transition setting the transition at its State and Input to null
        /// </summary>
        /// <param name="transition"></param>
        public bool Clear(BaseTransition oldTransition)
        {
            if(_transitions.TryGetValue(oldTransition.Origin, out var inputDictionary))
            {
                if(inputDictionary.TryUpdate(oldTransition.Input,
                                             new InvalidTransition(oldTransition.Origin,
                                                                   oldTransition.Input,
                                                                   "Undefined"),
                                             oldTransition))
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
        public BaseTransition Get(State state, Input input)
        {
            BaseTransition transition = null;
            if(_transitions.TryGetValue(state, out var inputDictionary))
            {
                inputDictionary.TryGetValue(input, out transition);
            }

            return transition;
        }

        public InputCollection Inputs { get; }

        public StateCollection States { get; }
    }
}