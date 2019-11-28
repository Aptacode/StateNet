using Aptacode.StateNet.FiniteStateMachine.Inputs;
using Aptacode.StateNet.FiniteStateMachine.States;
using System;

namespace Aptacode.StateNet.FiniteStateMachine.Exceptions
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