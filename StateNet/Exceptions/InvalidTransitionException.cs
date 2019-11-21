using System;

namespace Aptacode.StateNet.Exceptions
{
    public class InvalidTransitionException : Exception
    {
        public InvalidTransitionException() { }

        public InvalidTransitionException(string message) : base(message) { }

        public InvalidTransitionException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public InvalidTransitionException(string message, Exception innerException) : base(message, innerException) { }

        public string Input { get; set; }

        public string State { get; set; }
    }
}