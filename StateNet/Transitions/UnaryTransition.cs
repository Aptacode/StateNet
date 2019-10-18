using System;
using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.TransitionResult;

namespace Aptacode.StateNet.Transitions
{
    public class UnaryTransition : ValidTransition
    {
        /// <summary>
        ///     Defines a transition to the 'nextState' when the 'input' is applied to the current state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="nextState"></param>
        /// <param name="acceptanceCallback"></param>
        /// <param name="message"></param>
        public UnaryTransition(string state, string input, string nextState,
            Func<UnaryTransitionResult> acceptanceCallback, string message) : base(state, input, message)
        {
            NextState = nextState;
            AcceptanceCallback = acceptanceCallback;
        }

        /// <summary>
        ///     The output state of the unary transition
        /// </summary>
        public string NextState { get; }

        protected Func<UnaryTransitionResult> AcceptanceCallback { get; set; }

        /// <summary>
        ///     Apply the transition
        /// </summary>
        /// <returns></returns>
        public override string Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || !result.Success)
            {
                throw new AcceptanceCallbackFailedException(State, Input);
            }

            return NextState;
        }

        public override string ToString()
        {
            return $"Unary Transition: {State}({Input})->{NextState}";
        }
    }
}