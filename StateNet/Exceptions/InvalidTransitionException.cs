using Aptacode.StateNet.Inputs;
using Aptacode.StateNet.States;
using System;

namespace Aptacode.StateNet.Exceptions
{
    public class InvalidTransitionException : Exception
    {
        public InvalidTransitionException() { }

        public InvalidTransitionException(string message) : base(message) { }

        public InvalidTransitionException(State state, Input input)
        {
            State = state;
            Input = input;
        }

        public InvalidTransitionException(string message, Exception innerException) : base(message, innerException) { }

        public Input Input { get; set; }

        public State State { get; set; }
    }
}