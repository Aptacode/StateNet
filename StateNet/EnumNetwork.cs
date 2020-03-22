using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Connections;

namespace Aptacode.StateNet
{
    public class EnumNetwork<TStates, TInputs> : Network
        where TStates : Enum
        where TInputs : Enum
    {
        public State this[TStates state] => this[state.ToString()];
        public IEnumerable<Connection> this[TStates state, TInputs input] => this[state.ToString(), input.ToString()];

        public Connection this[TStates fromState, TInputs input, TStates toState]
        {
            get => this[fromState.ToString(), input.ToString(), toState.ToString()];
            set => this[fromState.ToString(), input.ToString(), toState.ToString()] = value;
        }

        public void SetStart(TStates state)
        {
            SetStart(state.ToString());
        }

        public Input GetInput(TInputs name, bool createIfMissing = true)
        {
            return GetInput(name.ToString(), createIfMissing);
        }

        public State GetState(TStates name, bool createIfMissing = true)
        {
            return GetState(name.ToString(), createIfMissing);
        }

        public IEnumerable<Input> GetInputs(TStates state)
        {
            return GetInputs(state.ToString());
        }

        public IEnumerable<Connection> GetConnections(TStates state)
        {
            return GetConnections(state.ToString());
        }

        public void Connect(TStates fromState, TInputs input, TStates toState, ConnectionWeight weight = null) =>
            this.Connect(fromState.ToString(), input.ToString(), toState.ToString(), weight);

        public void Always(TStates fromState, TInputs input, TStates toSate)
        {
            Always(fromState.ToString(), input.ToString(), toSate.ToString());
        }

        public void Clear(TStates fromState)
        {
            Clear(fromState.ToString());
        }

        public void Clear(TStates fromState, TInputs input)
        {
            Clear(fromState.ToString(), input.ToString());
        }

        public void Clear(TStates fromState, TInputs input, TStates toSate)
        {
            Clear(fromState.ToString(), input.ToString(), toSate.ToString());
        }

        public void SetDistribution(TStates fromState, TInputs action, params (TStates, int)[] choices)
        {
            SetDistribution(fromState.ToString(), action.ToString(),
                choices.Select(c => (c.Item1.ToString(), c.Item2)).ToArray());
        }

        public void SetDistribution(TStates fromState, TInputs action, params (TStates, ConnectionWeight)[] choices)
        {
            SetDistribution(fromState.ToString(), action.ToString(),
                choices.Select(c => (c.Item1.ToString(), c.Item2)).ToArray());
        }

        public void UpdateDistribution(TStates fromState, TInputs action, params (TStates, int)[] choices)
        {
            UpdateDistribution(fromState.ToString(), action.ToString(),
                choices.Select(c => (c.Item1.ToString(), c.Item2)).ToArray());
        }

        public void
            UpdateDistribution(TStates fromState, TInputs action, params (TStates, ConnectionWeight)[] choices)
        {
            UpdateDistribution(fromState.ToString(), action.ToString(),
                choices.Select(c => (c.Item1.ToString(), c.Item2)).ToArray());
        }

        public void RemoveState(TStates state)
        {
            RemoveState(state.ToString());
        }

        public void RemoveInput(TInputs input)
        {
            RemoveInput(input.ToString());
        }

        public Input CreateInput(TInputs input)
        {
            return CreateInput(input.ToString());
        }

        public State CreateState(TStates state)
        {
            return CreateState(state.ToString());
        }
    }
}