using System.Collections.Generic;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Interfaces
{
    public interface IStateNetworkEditor
    {
        #region States

        void SetStart(string name);
        State GetState(string name, bool createIfMissing = true);
        State CreateState(string name);
        void RemoveState(string name);

        IEnumerable<State> GetStates();
        IEnumerable<State> GetEndStates();

        #endregion

        #region Inputs

        Input GetInput(string name, bool createIfMissing = true);
        Input CreateInput(string name);
        void RemoveInput(string name);
        IEnumerable<Input> GetInputs();
        IEnumerable<Input> GetInputs(string state);

        #endregion

        #region Connections

        IEnumerable<Connection> GetConnections();
        IEnumerable<Connection> GetConnections(string source);
        IEnumerable<Connection> GetConnections(string source, string input);
        Connection GetConnection(string source, string input, string destination);

        void Always(string source, string input, string destination);
        void Clear(string source);
        void Clear(string source, string input);
        void Clear(string source, string input, string destination);
        void SetDistribution(string source, string input, params (string, int)[] choices);
        void SetDistribution(string source, string input, params (string, ConnectionWeight)[] choices);
        void UpdateDistribution(string source, string input, params (string, int)[] choices);
        void Connect(string source, string input, string destination, ConnectionWeight connectionWeight);
        void Disconnect(string source, string input, string destination);
        void UpdateDistribution(string source, string input, params (string, ConnectionWeight)[] choices);

        #endregion
    }
}