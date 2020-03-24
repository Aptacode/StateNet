using System;
using System.Collections.Generic;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Interfaces
{
    public interface IStateNetwork : IEquatable<IStateNetwork>
    {
        bool IsValid();

        #region States

        State StartState { get; }
        State this[string sourceState] { get; }
        void SetStart(string state);

        State CreateState(string state);
        State GetState(string stateName, bool createIfMissing = true);
        IEnumerable<State> GetStates();
        IEnumerable<State> GetEndStates();
        void RemoveState(string state);

        #endregion

        #region Inputs

        Input CreateInput(string input);
        Input GetInput(string inputName, bool createIfMissing = true);
        IEnumerable<Input> GetInputs();
        IEnumerable<Input> GetInputs(string state);
        void RemoveInput(string input);

        #endregion

        #region Connections

        IEnumerable<Connection> Connections { get; }
        IEnumerable<Connection> this[string sourceState, string input] { get; }
        Connection this[string sourceState, string input, string destinationState] { get; set; }
        IEnumerable<Connection> GetConnections();
        IEnumerable<Connection> GetConnections(string source);
        IEnumerable<Connection> GetConnections(string source, string input);
        Connection GetConnection(string source, string input, string destination);

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

        #endregion
    }
}