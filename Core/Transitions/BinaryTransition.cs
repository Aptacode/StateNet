using Aptacode.StateNet.Core.TransitionResult;
using Aptacode_StateMachine.StateNet.Core;
using System;

namespace Aptacode.StateNet.Core.Transitions
{

    /// <summary>
    /// Represents a transition to one of two states depending on the output of the transition
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <typeparam name="Action"></typeparam>
    public class BinaryTransition : ValidTransition
    {
        public string LeftState { get; private set; }
        public string RightState { get; private set; }
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
                case BinaryChoice.Left:
                default:
                    return LeftState;
            }
        }

        public override string ToString()
        {
            return string.Format("Binary Transition: {0}({1})->{2}|{3}", State, Input, LeftState, RightState);
        }
    }
}
