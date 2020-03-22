using System;
using System.Collections.Generic;
using Aptacode.StateNet.Connections;

namespace Aptacode.StateNet.Interfaces
{
    public interface INetwork : IEquatable<INetwork>
    {
        IEnumerable<Connection> Connections { get; }
        State StartState { get; }

        State this[string state] { get; }
        IEnumerable<Connection> this[string state, string input] { get; }
        Connection this[string fromState, string input, string toState] { get; set; }
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

        void Always(string fromState, string input, string toState);
        void Clear(string fromState);
        void Clear(string fromState, string input);
        void Clear(string fromState, string input, string toState);
        void SetDistribution(string fromState, string input, params (string, int)[] choices);
        void SetDistribution(string fromState, string input, params (string, ConnectionWeight)[] choices);
        void UpdateDistribution(string fromState, string input, params (string, int)[] choices);
        void Connect(string fromState, string input, string toState, ConnectionWeight weight);
        void UpdateDistribution(string fromState, string input,
            params (string, ConnectionWeight)[] choices);

        void RemoveState(string state);
        void RemoveInput(string input);

        Input CreateInput(string input);
        State CreateState(string state);
    }
}