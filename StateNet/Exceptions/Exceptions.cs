using System;
using Aptacode.StateNet.Transitions;

namespace Aptacode.StateNet.Exceptions
{
    public class DuplicateTransitionException : Exception
    {
        public Transition Transition { get; set; }
        public DuplicateTransitionException(Transition transition)
        {
            Transition = transition;
        }
    }

    public class InvalidTransitionException : Exception
    {
        public string State { get; set; }
        public string Input { get; set; }
        public InvalidTransitionException(string state, string input)
        {
            State = state;
            Input = input;
        }
    }

    public class UndefinedTransitionException : Exception
    {
        public string State { get; set; }
        public string Input { get; set; }
        public UndefinedTransitionException(string state, string input)
        {
            State = state;
            Input = input;
        }
    }

    public class AcceptanceCallbackFailedException : Exception
    {
        public string State { get; set; }
        public string Input { get; set; }
        public AcceptanceCallbackFailedException(string state, string input)
        {
            State = state;
            Input = input;
        }
    }
}
