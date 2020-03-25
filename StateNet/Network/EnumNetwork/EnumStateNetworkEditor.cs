using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.Network.EnumNetwork
{
    public class EnumStateNetworkEditor<TStates, TInputs> : StateNetworkEditor
        where TStates : Enum
        where TInputs : Enum
    {
        public EnumStateNetworkEditor(IStateNetwork stateNetwork) : base(stateNetwork)
        {
        }

        public IEnumerable<Connection> this[TStates state, TInputs input] => this[state.ToString(), input.ToString()];

        public Connection this[TStates source, TInputs input, TStates target]
        {
            get => this[source.ToString(), input.ToString(), target.ToString()];
            set => this[source.ToString(), input.ToString(), target.ToString()] = value;
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

        public void Connect(TStates source, TInputs input, TStates target, ConnectionWeight connectionWeight = null)
        {
            Connect(source.ToString(), input.ToString(), target.ToString(), connectionWeight);
        }

        public void Always(TStates source, TInputs input, TStates toSate)
        {
            Always(source.ToString(), input.ToString(), toSate.ToString());
        }

        public void Clear(TStates source)
        {
            Clear(source.ToString());
        }

        public void Clear(TStates source, TInputs input)
        {
            Clear(source.ToString(), input.ToString());
        }

        public void Clear(TStates source, TInputs input, TStates toSate)
        {
            Clear(source.ToString(), input.ToString(), toSate.ToString());
        }

        public void SetDistribution(TStates source, TInputs action, params (TStates, int)[] choices)
        {
            SetDistribution(source.ToString(), action.ToString(),
                choices.Select(c => (c.Item1.ToString(), c.Item2)).ToArray());
        }

        public void SetDistribution(TStates source, TInputs action, params (TStates, ConnectionWeight)[] choices)
        {
            SetDistribution(source.ToString(), action.ToString(),
                choices.Select(c => (c.Item1.ToString(), c.Item2)).ToArray());
        }

        public void UpdateDistribution(TStates source, TInputs action, params (TStates, int)[] choices)
        {
            UpdateDistribution(source.ToString(), action.ToString(),
                choices.Select(c => (c.Item1.ToString(), c.Item2)).ToArray());
        }

        public void
            UpdateDistribution(TStates source, TInputs action, params (TStates, ConnectionWeight)[] choices)
        {
            UpdateDistribution(source.ToString(), action.ToString(),
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