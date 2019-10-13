using System;

namespace Aptacode.StateNet.Core
{
    public class StateTransitionArgs : EventArgs
    {
        public string OldState { get; set; }
        public string NewState { get; set; }
        public string Input { get; set; }
        public StateTransitionArgs(string oldState, string action, string newState)
        {
            OldState = oldState;
            Input = action;
            NewState = newState;
        }

        public override string ToString()
        {
            return string.Format("{0}({1})->{2}", OldState, Input, NewState);
        }
    }
}
