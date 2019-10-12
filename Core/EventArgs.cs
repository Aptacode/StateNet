using System;

namespace Aptacode.StateNet.Core
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

        public override string ToString()
        {
            return string.Format("{0}({1})->{2}", Enum.GetName(typeof(States), OldState), Enum.GetName(typeof(Actions), Action), Enum.GetName(typeof(States), NewState));
        }
    }
}
