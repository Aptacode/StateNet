using System;
using Aptacode.StateNet.Transitions;

namespace Aptacode.StateNet.Exceptions
{
    public class DuplicateTransitionException : Exception
    {
        public DuplicateTransitionException(Transition transition)
        {
            Transition = transition;
        }

        public Transition Transition { get; set; }
    }

    public class InvalidTransitionException : Exception
    {
        public InvalidTransitionException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public string State { get; set; }
        public string Input { get; set; }
    }

    public class UndefinedTransitionException : Exception
    {
        public UndefinedTransitionException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public string State { get; set; }
        public string Input { get; set; }
    }

    public class AcceptanceCallbackFailedException : Exception
    {
        public AcceptanceCallbackFailedException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public string State { get; set; }
        public string Input { get; set; }
    }
}