using System;

namespace Aptacode_StateMachine.StateNet.Core
{
    public class StateTransitionArgs<States, Actions> : EventArgs
    {
        public States OldState { get; set; }
        public States NewState { get; set; }
        public Actions Action { get; set; }
        public StateTransitionArgs(States oldState, Actions action, States newState)
        {
            OldState = oldState;
            Action = action;
            NewState = newState;
        }
    }
}
