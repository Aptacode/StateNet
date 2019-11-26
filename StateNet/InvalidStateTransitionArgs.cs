using Aptacode.StateNet.Inputs;
using Aptacode.StateNet.States;
using System;

namespace Aptacode.StateNet
{
    public class InvalidStateTransitionArgs : EventArgs
    {
        public InvalidStateTransitionArgs(State state, Input input)
        {
            State = state;
            Input = input;
        }


        public override string ToString() => $"{State}({Input})->Invalid";

        public Input Input { get; set; }

        public State State { get; set; }
    }
}