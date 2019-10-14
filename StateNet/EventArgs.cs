using System;

namespace Aptacode.StateNet
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
            return $"{OldState}({Input})->{NewState}";
        }
    }
}
