using Aptacode.StateNet.Transitions;
using System;

namespace Aptacode.StateNet.Exceptions
{
    public class AcceptanceCallbackFailedException : Exception
    {
        public AcceptanceCallbackFailedException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public string State { get; set; }

        public string Input { get; set; }

        public AcceptanceCallbackFailedException()
        {
        }

        public AcceptanceCallbackFailedException(string message) : base(message)
        {
        }

        public AcceptanceCallbackFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}