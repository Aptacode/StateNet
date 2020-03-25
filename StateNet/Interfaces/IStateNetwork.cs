using System;
using System.Collections.Generic;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Interfaces
{
    public interface IStateNetwork : IEquatable<IStateNetwork>
    {
        bool IsValid();

        #region States

        State StartState { get; set; }
        State GetState(string name);
        State CreateState(string name);
        void RemoveState(string name);

        IEnumerable<State> GetStates();
        IEnumerable<State> GetEndStates();

        #endregion

        #region Inputs

        Input GetInput(string name);
        Input CreateInput(string name);
        void RemoveInput(string name);

        IEnumerable<Input> GetInputs();
        IEnumerable<Input> GetInputs(string state);

        #endregion

        #region Connections

        IEnumerable<Connection> Connections { get; }
        IEnumerable<Connection> this[string source] { get; }
        IEnumerable<Connection> this[string source, string input] { get; }
        Connection this[string source, string input, string destination] { get; set; }

        IEnumerable<Connection> GetConnections();
        IEnumerable<Connection> GetConnections(string source);
        IEnumerable<Connection> GetConnections(string source, string input);
        Connection GetConnection(string source, string input, string destination);

        void Connect(string source, string input, string destination, ConnectionWeight connectionWeight);
        void Disconnect(string source, string input, string destination);

        #endregion
    }
}