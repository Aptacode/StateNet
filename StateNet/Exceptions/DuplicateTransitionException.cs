using Aptacode.StateNet.Transitions;
using System;

namespace Aptacode.StateNet.Exceptions
{
    public class DuplicateTransitionException : Exception
    {
        public DuplicateTransitionException(Transition transition) => Transition = transition;

        public Transition Transition { get; set; }

        public DuplicateTransitionException()
        {
        }

        public DuplicateTransitionException(string message) : base(message)
        {
        }

        public DuplicateTransitionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}