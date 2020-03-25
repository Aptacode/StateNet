using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;

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
            _stateNetwork.StartState = GetState(name);
        }

        /// <summary>
        ///     Return the state with the given name
        ///     Create new state if missing
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createIfMissing"></param>
        /// <returns></returns>
        public State GetState(string name, bool createIfMissing = true)
        {
            return createIfMissing ? _stateNetwork.CreateState(name) : _stateNetwork.GetState(name);
        }

        /// <summary>
        ///     Return the state with the given name
        ///     Create new state if missing
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public State CreateState(string name)
        {
            return _stateNetwork.CreateState(name);
        }

        /// <summary>
        ///     Remove the state which matches the given name and all of its connections
        /// </summary>
        /// <param name="name"></param>
        public void RemoveState(string name)
        {
            _stateNetwork.RemoveState(name);
        }

        public IEnumerable<State> GetEndStates()
        {
            return _stateNetwork.GetEndStates();
        }

        public IEnumerable<State> GetStates()
        {
            return _stateNetwork.GetStates();
        }

        #endregion

        #region Inputs

        public IEnumerable<Input> GetInputs()
        {
            return _stateNetwork.GetInputs();
        }

        public IEnumerable<Input> GetInputs(string name)
        {
            return _stateNetwork.GetInputs(name);
        }

        public Input GetInput(string name, bool createIfMissing = true)
        {
            return createIfMissing ? _stateNetwork.CreateInput(name) : _stateNetwork.GetInput(name);
        }

        public void RemoveInput(string name)
        {
            _stateNetwork.RemoveInput(name);
        }

        public Input CreateInput(string name)
        {
            return _stateNetwork.CreateInput(name);
        }

        #endregion

        #region Connections

        public IEnumerable<Connection> GetConnections()
        {
            return _stateNetwork.GetConnections();
        }

        public IEnumerable<Connection> GetConnections(string source)
        {
            return _stateNetwork.GetConnections(source);
        }

        public IEnumerable<Connection> this[string source] => GetConnections(source);

        public IEnumerable<Connection> this[string source, string input] => GetConnections(source, input);

        public Connection this[string source, string input, string destination]
        {
            get => GetConnection(source, input, destination);
            set => Connect(value.From, value.Input, value.To, value.ConnectionWeight);
        }

        public void Connect(string source, string input, string destination,
            ConnectionWeight connectionWeight = null)
        {
            _stateNetwork.Connect(source, input, destination, connectionWeight);
        }

        public IEnumerable<Connection> GetConnections(string source, string input)
        {
            return _stateNetwork.GetConnections(source, input);
        }

        public Connection GetConnection(string source, string input, string destination)
        {
            return _stateNetwork.GetConnection(source, input, destination);
        }

        public void Disconnect(string source, string input, string destination)
        {
            _stateNetwork.Disconnect(source, input, destination);
        }

        public void Clear(string source)
        {
            GetConnections(source).ToList().ForEach(connection => connection?.From?.Remove(connection));
        }

        public void Clear(string source, string input)
        {
            GetConnections(source, input).ToList().ForEach(connection => connection?.From?.Remove(connection));
        }

        public void Clear(string source, string input, string destination)
        {
            var connection = GetConnection(source, input, destination);
            connection?.From?.Remove(connection);
        }

        public void Always(string source, string input, string destination)
        {
            Clear(source, input);
            Connect(source, input, destination, new ConnectionWeight(1.ToString()));
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
            foreach (var (destination, weight) in choices)
            {
                Clear(source, input, destination);
                Connect(source, input, destination, weight);
            }
        }

        #endregion
    }
}