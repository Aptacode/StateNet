using Aptacode.StateNet.FiniteStateMachine.Inputs;
using Aptacode.StateNet.FiniteStateMachine.States;
using Aptacode.StateNet.FiniteStateMachine.Transitions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.FiniteStateMachine
{
    public class TransitionTable
    {
        protected readonly ConcurrentDictionary<State, ConcurrentDictionary<Input, BaseTransition>> _transitions;

        public TransitionTable(StateCollection states, InputCollection inputs)
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

        /// <summary>
        /// Define a transition
        /// </summary>
        /// <param name="transition"></param>
        public bool Set(BaseTransition newTransition)
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

        public void Set(State fromState, Input input, string message)
        {
            var transition = new InvalidTransition(fromState, input, message);
            this.Set(transition);
        }

        public void Set(State fromState, Input input, State toState, string message)
        {
            var transition = new Transition(fromState, input, toState, message);
            this.Set(transition);
        }

        public void Set(State fromState,
                        Input input,
                        State toState1,
                        State toState2,
                        Func<(State, State), State> choiceFunction,
                        string message)
        {
            var transition = new Transition<(State, State)>(fromState,
                                                            input,
                                                            (toState1, toState2),
                                                            choiceFunction,
                                                            message);
            this.Set(transition);
        }

        public void Set(State fromState,
                        Input input,
                        State toState1,
                        State toState2,
                        Action<(State, State), Dictionary<State, int>> choiceFunction,
                        string message)
        {
            var transition = new ProbabilityTransition<(State, State)>(fromState,
                                                                       input,
                                                                       (toState1, toState2),
                                                                       choiceFunction,
                                                                       message);
            this.Set(transition);
        }

        public void Set(State fromState,
                        Input input,
                        State toState1,
                        State toState2,
                        State toState3,
                        Func<(State, State, State), State> choiceFunction,
                        string message)
        {
            var transition = new Transition<(State, State, State)>(fromState,
                                                                   input,
                                                                   (toState1,
                                                                                                             toState2,
                                                                                                             toState3),
                                                                   choiceFunction,
                                                                   message);
            this.Set(transition);
        }

        public void Set(State fromState,
                        Input input,
                        State toState1,
                        State toState2,
                        State toState3,
                        State toState4,
                        Func<(State, State, State, State), State> choiceFunction,
                        string message)
        {
            var transition = new Transition<(State, State, State, State)>(fromState,
                                                                          input,
                                                                          (toState1,
                                                                                    toState2,
                                                                                    toState3,
                                                                                    toState4),
                                                                          choiceFunction,
                                                                          message);
            this.Set(transition);
        }

        public InputCollection Inputs { get; }

        public StateCollection States { get; }
    }
}