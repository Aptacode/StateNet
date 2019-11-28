using Aptacode.StateNet.FiniteStateMachine.Inputs;
using Aptacode.StateNet.FiniteStateMachine.States;
using System;

namespace Aptacode.StateNet.FiniteStateMachine
{
    public class StateTransitionArgs : EventArgs
    {
        public StateTransitionArgs(State oldState, Input action, State newState)
        {
            OldState = oldState;
            Input = action;
            NewState = newState;
        }

        public override string ToString() => $"{OldState}({Input})->{NewState}" ;

        public Input Input { get; set; }

        public State NewState { get; set; }

        public State OldState { get; set; }
    }
}