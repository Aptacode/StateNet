using Aptacode.StateNet.Exceptions;

namespace Aptacode.StateNet.Transitions
{
    public class InvalidTransition : Transition
    {
        /// <summary>
        /// A transition which CAN NOT exist
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="message"></param>
        public InvalidTransition(string state, string input, string message) : base(state, input, message)
        {
        }

        /// <summary>
        /// Throws an exception as an invalidTransition cannot be applied.
        /// </summary>
        /// <returns></returns>
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