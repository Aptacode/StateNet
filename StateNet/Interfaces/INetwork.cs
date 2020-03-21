using System;
using System.Collections.Generic;
using Aptacode.StateNet.Connections;

namespace Aptacode.StateNet.Interfaces
{
    public interface INetwork : IEquatable<INetwork>
    {
        List<Connection> Connections { get; }

        State StartState { get; }

        State this[string state] { get; }
        IEnumerable<Connection> this[string state, string action] { get; }
        Connection this[string fromState, string action, string toState] { get; set; }
        void SetStart(string state);


        IEnumerable<State> GetStates();

        IEnumerable<State> GetEndStates();

        bool IsValid();

        Input GetInput(string name, bool createIfMissing = true);
        IEnumerable<Input> GetInputs();
        IEnumerable<Input> GetInputs(string state);

        IEnumerable<Connection> GetConnections();
    }
}