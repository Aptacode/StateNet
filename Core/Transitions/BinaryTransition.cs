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
    public class BinaryTransition<States, Actions> : ValidTransition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public States LeftState { get; private set; }
        public States RightState { get; private set; }
        protected Func<BinaryTransitionResult> AcceptanceCallback { get; set; }

        public BinaryTransition(States state, Actions action, States leftState, States rightState, Func<BinaryTransitionResult> acceptanceCallback, string message) : base(state, action, message)
        {
            LeftState = leftState;
            RightState = rightState;
            AcceptanceCallback = acceptanceCallback;
        }

        public override States Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || !result.Success)
                throw new AcceptanceCallbackFailedException<States, Actions>(State, Action);

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
            return string.Format("Binary Transition: {0}({1})->{2}|{3}", Enum.GetName(typeof(States), State), Enum.GetName(typeof(Actions), Action), Enum.GetName(typeof(States), LeftState), Enum.GetName(typeof(States), RightState));
        }
    }
}
