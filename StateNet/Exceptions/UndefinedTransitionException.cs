using System;

namespace Aptacode.StateNet.Exceptions
{
    public class UndefinedTransitionException : Exception
    {
        public UndefinedTransitionException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public string State { get; set; }

        public string Input { get; set; }

        public UndefinedTransitionException()
        {
        }

        public UndefinedTransitionException(string message) : base(message)
        {
        }

        public UndefinedTransitionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}