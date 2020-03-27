using System.Collections.Generic;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Interfaces
{
    public interface IStateNetworkEditor
    {
        #region States

        void SetStart(string name);
        State GetState(string name, bool createIfMissing);
        State GetState(string name);

        State CreateState(string name);
        void RemoveState(string name);

        IEnumerable<State> GetStates();
        IEnumerable<State> GetEndStates();

        #endregion

        #region Inputs

        Input GetInput(string name, bool createIfMissing);
        Input GetInput(string name);

        Input CreateInput(string name);
        void RemoveInput(string name);
        IEnumerable<Input> GetInputs();
        IEnumerable<Input> GetInputs(string state);

        #endregion

        #region Connections

        IEnumerable<Connection> GetConnections();
        IEnumerable<Connection> GetConnections(string source);
        IEnumerable<Connection> GetConnections(string source, string input);
        Connection GetConnection(string source, string input, string target);

        void Always(string source, string input, string target);
        void Clear(string source);
        void Clear(string source, string input);
        void Clear(string source, string input, string target);
        void SetDistribution(string source, string input, params (string, int)[] choices);
        void SetDistribution(string source, string input, params (string, ConnectionWeight)[] choices);
        void UpdateDistribution(string source, string input, params (string, int)[] choices);
        void Connect(string source, string input, string target);
        void Connect(string source, string input, string target, ConnectionWeight connectionWeight);
        void Disconnect(string source, string input, string target);
        void UpdateDistribution(string source, string input, params (string, ConnectionWeight)[] choices);

        #endregion
    }
}