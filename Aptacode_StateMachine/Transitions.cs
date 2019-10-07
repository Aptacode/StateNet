using System;

namespace Aptacode_StateMachine
{
    /// <summary>
    /// An abstract generic class which represents a transition from 'State' With 'Action'
    /// </summary>
    /// <typeparam name="States">an Enum containing the available States</typeparam>
    /// <typeparam name="Actions">an Enum containing the available actions</typeparam>
    public abstract class Transition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public States State { get; private set; }
        public Actions Action { get; private set; }
        public string Message { get; set; }

        public Transition(States state, Actions action, string message = "")
        {
            State = state;
            Action = action;
            Message = message;
        }

        public abstract override string ToString();

        public abstract States Apply();
    }

    /// <summary>
    /// A State Transition which is prohibited
    /// </summary>
    /// <typeparam name="States">an Enum containing the available States</typeparam>
    /// <typeparam name="Actions">an Enum containing the available actions</typeparam>
    public class InvalidTransition<States, Actions> : Transition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public InvalidTransition(States state, Actions action, string message = "Invalid Transition") : base(state, action, message)
        {

        }

        public override States Apply()
        {
            throw new InvalidTransitionException<States, Actions>(State, Action);
        }

        public override string ToString()
        {
            return string.Format("Invalid Transition: {0}({1})", Enum.GetName(typeof(States), State), Enum.GetName(typeof(Actions), Action));
        }
    }

    /// <summary>
    /// Represents an abstract valid state transition, this class must be inherited to define the state to transition to.
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <typeparam name="Action"></typeparam>
    public abstract class ValidTransition<States, Actions> : Transition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public ValidTransition(States state, Actions action, string message = "Valid Transition") : base(state, action, message)
        {

        }
    }

    /// <summary>
    /// Represents a transition to exactly one state
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <typeparam name="Action"></typeparam>
    public class UnaryTransition<States, Actions> : ValidTransition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public States NextState { get; private set; }
        protected Func<UnaryTransitionAcceptanceResult> AcceptanceCallback { get; set; }

        public UnaryTransition(States state, Actions action, States nextState, Func<UnaryTransitionAcceptanceResult> acceptanceCallback, string message = "Unary Transition") : base(state, action, message)
        {
            NextState = nextState;
            AcceptanceCallback = acceptanceCallback;
        }

        public override States Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || result.Failed)
                throw new AcceptanceCallbackFailedException<States, Actions>();

            return NextState;
        }

        public override string ToString()
        {
            return string.Format("Unary Transition: {0}({1})->{2}", Enum.GetName(typeof(States), State), Enum.GetName(typeof(Actions), Action), Enum.GetName(typeof(States), NextState));
        }
    }

    /// <summary>
    /// Represents a transition to one of two states depending on the output of the transition
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <typeparam name="Action"></typeparam>
    public class BinaryTransition<States, Actions> : ValidTransition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public States LeftState { get; private set; }
        public States RightState { get; private set; }
        protected Func<BinaryTransitionAcceptanceResult> AcceptanceCallback { get; set; }

        public BinaryTransition(States state, Actions action, States leftState, States rightState, Func<BinaryTransitionAcceptanceResult> acceptanceCallback, string p_Message = "Binary Transition") : base(state, action, p_Message)
        {
            LeftState = leftState;
            RightState = rightState;
            AcceptanceCallback = acceptanceCallback;
        }

        public override States Apply()
        {
            var result = AcceptanceCallback?.Invoke();
            if (result == null || result.Failed)
                throw new AcceptanceCallbackFailedException<States, Actions>();

            switch (result.Choice)
            {
                case BinaryTransitionAcceptanceResult.BinaryChoice.Right:
                    return RightState;
                case BinaryTransitionAcceptanceResult.BinaryChoice.Left:
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
