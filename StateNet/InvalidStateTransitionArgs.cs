using System;

namespace Aptacode.StateNet
{
    public class InvalidStateTransitionArgs : EventArgs
    {
        public InvalidStateTransitionArgs(string state, string input)
        {
            State = state;
            Input = input;
        }


        public override string ToString() => $"{State}({Input})->Invalid";

        public string Input { get; set; }

        public string State { get; set; }
    }
}