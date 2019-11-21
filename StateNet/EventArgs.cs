using System;

namespace Aptacode.StateNet
{
    public class StateTransitionArgs : EventArgs
    {
        public StateTransitionArgs(string oldState, string action, string newState)
        {
            OldState = oldState;
            Input = action;
            NewState = newState;
        }

        public override string ToString() => $"{OldState}({Input})->{NewState}" ;

        public string Input { get; set; }

        public string NewState { get; set; }

        public string OldState { get; set; }
    }
}