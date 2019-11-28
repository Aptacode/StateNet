using Aptacode.StateNet.TableMachine.Inputs;
using Aptacode.StateNet.TableMachine.States;
using System;

namespace Aptacode.StateNet.TableMachine.Events
{
    public class StateTransitionArgs : EventArgs
    {
        public StateTransitionArgs(State oldState, Input input, State newState)
        {
            OldState = oldState;
            Input = input;
            NewState = newState;
        }

        public override string ToString() => $"{OldState}({Input})->{NewState}";

        public Input Input { get; set; }

        public State NewState { get; set; }

        public State OldState { get; set; }
    }
}