using System;
using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.TransitionResult;

namespace Aptacode.StateNet.Transitions
{
    public class BinaryTransition : ValidTransition
    {
        public string LeftState { get; }
        public string RightState { get; }
        protected Func<BinaryTransitionResult> AcceptanceCallback { get; set; }

        public BinaryTransition(string state, string input, string leftState, string rightState, Func<BinaryTransitionResult> acceptanceCallback, string message) : base(state, input, message)
        {
            LeftState = leftState;
            RightState = rightState;
            AcceptanceCallback = acceptanceCallback;
        }

        public override string Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || !result.Success)
                throw new AcceptanceCallbackFailedException(State, Input);

            switch (result.Choice)
            {
                case BinaryChoice.Right:
                    return RightState;
                default:
                    return LeftState;
            }
        }

        public override string ToString()
        {
            return $"Binary Transition: {State}({Input})->{LeftState}|{RightState}";
        }
    }
}
