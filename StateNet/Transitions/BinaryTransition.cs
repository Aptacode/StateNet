using System;
using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.TransitionResult;

namespace Aptacode.StateNet.Transitions
{
    public class BinaryTransition : ValidTransition
    {
        public BinaryTransition(string state, string input, string leftState, string rightState,
            Func<BinaryTransitionResult> acceptanceCallback, string message) : base(state, input, message)
        {
            LeftState = leftState;
            RightState = rightState;
            AcceptanceCallback = acceptanceCallback;
        }

        public string LeftState { get; }
        public string RightState { get; }
        protected Func<BinaryTransitionResult> AcceptanceCallback { get; set; }

        public override string Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || !result.Success) throw new AcceptanceCallbackFailedException(State, Input);

            if (result.Choice == BinaryChoice.Left)
                return LeftState;
            return RightState;
        }

        public override string ToString()
        {
            return $"Binary Transition: {State}({Input})->{LeftState}|{RightState}";
        }
    }
}