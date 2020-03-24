using System;
using System.Collections.Generic;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Interfaces
{
    public interface IStateNetwork : IEquatable<IStateNetwork>
    {
        IEnumerable<Connection> Connections { get; }
        State StartState { get; }

        State this[string sourceState] { get; }
        IEnumerable<Connection> this[string sourceState, string input] { get; }
        Connection this[string sourceState, string input, string destinationState] { get; set; }
        void SetStart(string state);

        IEnumerable<State> GetStates();

        IEnumerable<State> GetEndStates();

        bool IsValid();
        State GetState(string stateName, bool createIfMissing = true);
        Input GetInput(string inputName, bool createIfMissing = true);
        IEnumerable<Input> GetInputs();
        IEnumerable<Input> GetInputs(string state);

        IEnumerable<Connection> GetConnections();
        IEnumerable<Connection> GetConnections(string state);
        IEnumerable<Connection> GetConnections(State state, string input);

        void Always(string sourceState, string input, string destinationState);
        void Clear(string sourceState);
        void Clear(string sourceState, string input);
        void Clear(string sourceState, string input, string destinationState);
        void SetDistribution(string sourceState, string input, params (string, int)[] choices);
        void SetDistribution(string sourceState, string input, params (string, ConnectionWeight)[] choices);
        void UpdateDistribution(string sourceState, string input, params (string, int)[] choices);
        void Connect(string sourceState, string input, string destinationState, ConnectionWeight connectionWeight);

        void UpdateDistribution(string sourceState, string input,
            params (string, ConnectionWeight)[] choices);

        void RemoveState(string state);
        void RemoveInput(string input);

        Input CreateInput(string input);
        State CreateState(string state);
    }
}