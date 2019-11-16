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

        public string OldState { get; set; }

        public string NewState { get; set; }

        public string Input { get; set; }

        public override string ToString() => $"{OldState}({Input})->{NewState}" ;
    }
}