using Aptacode.StateNet.Exceptions;

namespace Aptacode.StateNet.Transitions
{
    public class InvalidTransition : Transition
    {
        public InvalidTransition(string state, string input, string message) : base(state, input, message)
        {
        }

        public override string Apply()
        {
            throw new InvalidTransitionException(State, Input);
        }

        public override string ToString()
        {
            return $"Invalid Transition: {State}({Input})";
        }
    }
}