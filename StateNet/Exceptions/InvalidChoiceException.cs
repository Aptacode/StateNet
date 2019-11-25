using System;

namespace Aptacode.StateNet.Exceptions
{
    public class InvalidChoiceException : Exception
    {
        public InvalidChoiceException() { }

        public InvalidChoiceException(string message) : base(message) { }

        public InvalidChoiceException(string state, string input)
        {
            State = state;
            Input = input;
        }

        public InvalidChoiceException(string message, Exception innerException) : base(message, innerException) { }

        public string Input { get; set; }

        public string State { get; set; }
    }
}