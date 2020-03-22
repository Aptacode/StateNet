using System;
using System.Collections.Generic;
using Aptacode.StateNet.Connections;

namespace Aptacode.StateNet.Interfaces
{
    public interface INetwork : IEquatable<INetwork>
    {
        IEnumerable<Connection> Connections { get; }
        void SetStart(string state);
        State StartState { get; }

        State this[string state] { get; }
        IEnumerable<Connection> this[string state, string input] { get; }
        Connection this[string fromState, string input, string toState] { get; set; }

        IEnumerable<State> GetStates();

        IEnumerable<State> GetEndStates();

        bool IsValid();

        Input GetInput(string name, bool createIfMissing = true);
        IEnumerable<Input> GetInputs();
        IEnumerable<Input> GetInputs(string state);

        IEnumerable<Connection> GetConnections();
        IEnumerable<Connection> GetConnections(string state);
    }
}