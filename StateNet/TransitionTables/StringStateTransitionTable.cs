using Aptacode.StateNet.Transitions;
using System;
using System.Linq;


namespace Aptacode.StateNet.TransitionTables
{
    public class StringStateTransitionTable : StateTransitionTable
    {
        public StringStateTransitionTable(StateCollection states, InputCollection inputs) : base(states, inputs) { }

        public void Set(string fromState, string input, string message)
        {
            var transition = new InvalidTransition(fromState, input, message);
            this.Set(transition);
        }

        public void Set(string fromState, string input, string toState, string message)
        {
            var transition = new Transition<Tuple<string>>(fromState,
                                                           input,
                                                           new Tuple<string>(toState),
                                                           (states) => states.Item1,
                                                           message);
            this.Set(transition);
        }

        public void Set(string fromState,
                        string input,
                        string toState1,
                        string toState2,
                        Func<Tuple<string, string>, string> choiceFunction,
                        string message)
        {
            var transition = new Transition<Tuple<string, string>>(fromState,
                                                                   input,
                                                                   new Tuple<string, string>(toState1, toState2),
                                                                   choiceFunction,
                                                                   message);
            this.Set(transition);
        }

        public void Set(string fromState,
                        string input,
                        string toState1,
                        string toState2,
                        string toState3,
                        Func<Tuple<string, string, string>, string> choiceFunction,
                        string message)
        {
            var transition = new Transition<Tuple<string, string, string>>(fromState,
                                                                           input,
                                                                           new Tuple<string, string, string>(toState1,
                                                                                                             toState2,
                                                                                                             toState3),
                                                                           choiceFunction,
                                                                           message);
            this.Set(transition);
        }

        public void Set(string fromState,
                        string input,
                        string toState1,
                        string toState2,
                        string toState3,
                        string toState4,
                        Func<Tuple<string, string, string, string>, string> choiceFunction,
                        string message)
        {
            var transition = new Transition<Tuple<string, string, string, string>>(fromState,
                                                                                   input,
                                                                                   new Tuple<string, string, string, string>(toState1,
                                                                                                                             toState2,
                                                                                                                             toState3,
                                                                                                                             toState4),
                                                                                   choiceFunction,
                                                                                   message);
            this.Set(transition);
        }
    }
}