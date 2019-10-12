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
    public class UnaryTransition<States, Actions> : ValidTransition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public States NextState { get; private set; }
        protected Func<UnaryTransitionResult> AcceptanceCallback { get; set; }

        public UnaryTransition(States state, Actions action, States nextState, Func<UnaryTransitionResult> acceptanceCallback, string message) : base(state, action, message)
        {
            NextState = nextState;
            AcceptanceCallback = acceptanceCallback;
        }

        public override States Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || !result.Success)
                throw new AcceptanceCallbackFailedException<States, Actions>(State, Action);

            return NextState;
        }

        public override string ToString()
        {
            return string.Format("Unary Transition: {0}({1})->{2}", Enum.GetName(typeof(States), State), Enum.GetName(typeof(Actions), Action), Enum.GetName(typeof(States), NextState));
        }
    }

}
