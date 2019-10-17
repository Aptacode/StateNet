using System;
using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.TransitionResult;

namespace Aptacode.StateNet.Transitions
{
    public class UnaryTransition : ValidTransition
    {
        public UnaryTransition(string state, string input, string nextState,
            Func<UnaryTransitionResult> acceptanceCallback, string message) : base(state, input, message)
        {
            NextState = nextState;
            AcceptanceCallback = acceptanceCallback;
        }

        public string NextState { get; }
        protected Func<UnaryTransitionResult> AcceptanceCallback { get; set; }

        public override string Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || !result.Success)
                throw new AcceptanceCallbackFailedException(State, Input);

            return NextState;
        }

        public override string ToString()
        {
            return $"Unary Transition: {State}({Input})->{NextState}";
        }
    }
}