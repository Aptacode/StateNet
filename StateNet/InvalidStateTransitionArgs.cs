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
        public string State { get; set; }

        public string Input { get; set; }


        public override string ToString() => $"{State}({Input})->Invalid";
    }
}