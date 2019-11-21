using System;

namespace Aptacode.StateNet.Exceptions
{
    public class AcceptanceCallbackFailedException : Exception
    {
        public AcceptanceCallbackFailedException() { }

        public AcceptanceCallbackFailedException(string message) : base(message) { }

        public AcceptanceCallbackFailedException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public AcceptanceCallbackFailedException(string message, Exception innerException) : base(message,
                                                                                                  innerException)
        { }

        public string Input { get; set; }

        public string State { get; set; }
    }
}