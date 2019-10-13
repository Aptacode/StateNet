using Aptacode.StateNet.Core.TransitionResult;
using Aptacode_StateMachine.StateNet.Core;
using System;

namespace Aptacode.StateNet.Core.Transitions
{
    /// <summary>
    /// Represents a transition to exactly one state
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <typeparam name="Action"></typeparam>
    public class UnaryTransition : ValidTransition
    {
        public string NextState { get; private set; }
        protected Func<UnaryTransitionResult> AcceptanceCallback { get; set; }

        public UnaryTransition(string state, string input, string nextState, Func<UnaryTransitionResult> acceptanceCallback, string message) : base(state, input, message)
        {
            NextState = nextState;
            AcceptanceCallback = acceptanceCallback;
        }

        public override string Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || !result.Success)
                throw new AcceptanceCallbackFailedException(State, Input);

            return NextState;
        }

        public override string ToString()
        {
            return string.Format("Unary Transition: {0}({1})->{2}", State, Input, NextState);
        }
    }

}
