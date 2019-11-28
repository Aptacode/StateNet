using Aptacode.StateNet.FiniteStateMachine.Inputs;
using Aptacode.StateNet.FiniteStateMachine.States;
using System;

namespace Aptacode.StateNet.FiniteStateMachine.Exceptions
{
    public class InvalidChoiceException : Exception
    {
        public InvalidChoiceException() { }

        public InvalidChoiceException(string message) : base(message) { }

        public InvalidChoiceException(State state, Input input)
        {
            State = state;
            Input = input;
        }

        public InvalidChoiceException(string message, Exception innerException) : base(message, innerException) { }

        public Input Input { get; set; }

        public State State { get; set; }
    }
}