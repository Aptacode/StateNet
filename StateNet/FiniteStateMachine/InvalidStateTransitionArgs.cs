using Aptacode.StateNet.FiniteStateMachine.Inputs;
using Aptacode.StateNet.FiniteStateMachine.States;
using System;

namespace Aptacode.StateNet.FiniteStateMachine
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