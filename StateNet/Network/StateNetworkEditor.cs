using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network.Connections;

namespace Aptacode.StateNet.Network
{
    public class StateNetworkEditor : IStateNetworkEditor
    {
        private readonly IStateNetwork _stateNetwork;

        public StateNetworkEditor(IStateNetwork stateNetwork)
        {
            _stateNetwork = stateNetwork;
        }

        #region States

        public void SetStart(string name)
        {
            _stateNetwork.StartState = GetState(name, true);
        }

        /// <summary>
        ///     Return the state with the given name
        ///     Create new state if missing
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createIfMissing"></param>
        /// <returns></returns>
        public State GetState(string name, bool createIfMissing) =>
            createIfMissing ? _stateNetwork.CreateState(name) : _stateNetwork.GetState(name);

        public State GetState(string name) => GetState(name, true);

        /// <summary>
        ///     Return the state with the given name
        ///     Create new state if missing
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public State CreateState(string name) => _stateNetwork.CreateState(name);

        /// <summary>
        ///     Remove the state which matches the given name and all of its connections
        /// </summary>
        /// <param name="name"></param>
        public void RemoveState(string name)
        {
            _stateNetwork.RemoveState(name);
        }

        public IEnumerable<State> GetEndStates() => _stateNetwork.GetEndStates();

        public IEnumerable<State> GetStates() => _stateNetwork.GetStates();

        #endregion

        #region Inputs

        public IEnumerable<Input> GetInputs() => _stateNetwork.GetInputs();

        public IEnumerable<Input> GetInputs(string name) => _stateNetwork.GetInputs(name);

        public Input GetInput(string name, bool createIfMissing) =>
            createIfMissing ? _stateNetwork.CreateInput(name) : _stateNetwork.GetInput(name);

        public Input GetInput(string name) => GetInput(name, true);

        public void RemoveInput(string name)
        {
            _stateNetwork.RemoveInput(name);
        }

        public Input CreateInput(string name) => _stateNetwork.CreateInput(name);

        #endregion

        #region Connections

        public IEnumerable<Connection> GetConnections() => _stateNetwork.GetConnections();

        public IEnumerable<Connection> GetConnections(string source) => _stateNetwork.GetConnections(source);

        public IEnumerable<Connection> this[string source] => GetConnections(source);

        public IEnumerable<Connection> this[string source, string input] => GetConnections(source, input);

        public Connection this[string source, string input, string target]
        {
            get => GetConnection(source, input, target);
            set => Connect(value.Source, value.Input, value.Target, value.ConnectionWeight);
        }

        public void Connect(string source, string input, string target,
            ConnectionWeight connectionWeight)
        {
            _stateNetwork.Connect(GetState(source, true), GetInput(input, true), GetState(target, true),
                connectionWeight);
        }

        public void Connect(string source, string input, string target)
        {
            Connect(source, input, target, new ConnectionWeight(1));
        }

        public IEnumerable<Connection> GetConnections(string source, string input) =>
            _stateNetwork.GetConnections(source, input);

        public Connection GetConnection(string source, string input, string target) =>
            _stateNetwork.GetConnection(source, input, target);

        public void Disconnect(string source, string input, string target)
        {
            _stateNetwork.Disconnect(source, input, target);
        }

        public void Clear(string source)
        {
            GetConnections(source).ToList().ForEach(connection => connection?.Source?.Remove(connection));
        }

        public void Clear(string source, string input)
        {
            GetConnections(source, input).ToList().ForEach(connection => connection?.Source?.Remove(connection));
        }

        public void Clear(string source, string input, string target)
        {
            var connection = GetConnection(source, input, target);
            connection?.Source?.Remove(connection);
        }

        public void Always(string source, string input, string target)
        {
            Clear(source, input);
            Connect(source, input, target);
        }

        public void SetDistribution(string source, string input, params (string, int)[] choices)
        {
            Clear(source, input);
            UpdateDistribution(source, input, choices);
        }

        public void SetDistribution(string source, string input, params (string, ConnectionWeight)[] choices)
        {
            Clear(source, input);
            UpdateDistribution(source, input, choices);
        }

        public void UpdateDistribution(string source, string input, params (string, int)[] choices)
        {
            var connectionWeights = choices
                .Select(c => (c.Item1, new ConnectionWeight(c.Item2.ToString())))
                .ToArray();
            UpdateDistribution(source, input, connectionWeights);
        }

        public void UpdateDistribution(string source, string input,
            params (string, ConnectionWeight)[] choices)
        {
            foreach (var (target, weight) in choices)
            {
                Clear(source, input, target);
                Connect(source, input, target, weight);
            }
        }

        #endregion
    }
}