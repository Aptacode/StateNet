using Aptacode.StateNet.TransitionResults;
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
            var transition = new UnaryTransition(fromState, input, toState, message);
            this.Set(transition);
        }

        public void Set(string fromState,
                        string input,
                        Func<int> transitionChoice,
                        string message,
                        params string[] toStates)
        {
            var transition = new NaryTransition(fromState,
                                                input,
                                                toStates.Select(state => state).ToList(),
                                                transitionChoice,
                                                message);
            this.Set(transition);
        }

        public void Set(string fromState,
                        string input,
                        string toState1,
                        string toState2,
                        Func<BinaryChoice> transitionChoice,
                        string message = "Binary")
        {
            var transition = new BinaryTransition(fromState,
                                                  input,
                                                  toState1,
                                                  toState2,
                                                  transitionChoice,
                                                  message);
            this.Set(transition);
        }
    }
}