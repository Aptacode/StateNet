using System;
using System.Collections.Generic;
using Aptacode.StateNet.TableMachine.Inputs;
using Aptacode.StateNet.TableMachine.States;
using Aptacode.StateNet.TableMachine.Transitions;

namespace Aptacode.StateNet.TableMachine.Tables
{
    public class NonDeterministicTransitionTable : TransitionTable
    {
        public NonDeterministicTransitionTable(StateCollection states, InputCollection inputs) : base(states, inputs)
        { }

        public void Set(State fromState, Input input, string message)
        {
            var transition = new InvalidTransition(fromState, input, message);
            Set(transition);
        }

        public void Set(State fromState, Input input, State toState, string message)
        {
            var transition = new Transition(fromState, input, toState, message);
            Set(transition);
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
            Set(transition);
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
            Set(transition);
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
            Set(transition);
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
            Set(transition);
        }
    }
}