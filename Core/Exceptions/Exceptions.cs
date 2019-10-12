using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;

namespace Aptacode.StateNet.Core
{
    public class DuplicateTransitionException<States, Actions> : Exception where States : struct, Enum where Actions : struct, Enum
    {
        public Transition<States, Actions> Transition { get; set; }
        public DuplicateTransitionException(Transition<States, Actions> transition)
        {
            Transition = transition;
        }
    }

    public class InvalidTransitionException<States, Actions> : Exception where States : struct, Enum where Actions : struct, Enum
    {
        public States State { get; set; }
        public Actions Action { get; set; }
        public InvalidTransitionException(States state, Actions action)
        {
            State = state;
            Action = action;
        }
    }

    public class UndefinedTransitionException<States, Actions> : Exception where States : struct, Enum where Actions : struct, Enum
    {
        public States State { get; set; }
        public Actions Action { get; set; }
        public UndefinedTransitionException(States state, Actions action)
        {
            State = state;
            Action = action;
        }
    }

    public class AcceptanceCallbackFailedException<States, Actions> : Exception where States : struct, Enum where Actions : struct, Enum
    {
        public States State { get; set; }
        public Actions Action { get; set; }
        public AcceptanceCallbackFailedException(States state, Actions action)
        {
            State = state;
            Action = action;
        }
    }
}
