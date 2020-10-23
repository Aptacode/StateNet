using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Network
{
    public class NetworkBuilder
    {
        private readonly List<(string, string, Connection)> _connections;
        private readonly HashSet<string> _inputs;
        private readonly HashSet<string> _states;
        private string _startState;

        protected NetworkBuilder()
        {
            _startState = string.Empty;
            _states = new HashSet<string>();
            _inputs = new HashSet<string>();
            _connections = new List<(string, string, Connection)>();
        }

        public static NetworkBuilder New => new NetworkBuilder();

        public NetworkBuilder SetStartState(string startState)
        {
            _startState = startState;
            return this;
        }

        public NetworkBuilder AddState(string state)
        {
            _states.Add(state);
            return this;
        }

        public NetworkBuilder RemoveState(string state)
        {
            ClearConnectionsFromState(state);
            ClearConnectionsToState(state);
            _states.Remove(state);
            return this;
        }

        public NetworkBuilder AddInput(string input)
        {
            _inputs.Add(input);
            return this;
        }

        public NetworkBuilder RemoveInput(string input)
        {
            ClearConnectionsWithInput(input);
            _inputs.Remove(input);
            return this;
        }

        public NetworkBuilder AddConnection(string source, string input, string destination,
            Expression<Func<TransitionHistory, int>> expression)
        {
            AddState(source);
            AddInput(input);
            AddState(destination);

            _connections.Add((source, input, new Connection(destination, expression)));
            return this;
        }

        public NetworkBuilder AddConnection(string source, string input, string destination, int staticWeight)
        {
            return AddConnection(source, input, destination, _ => staticWeight);
        }

        public NetworkBuilder ClearConnectionsFromState(string state, string input)
        {
            var connectionToRemove = _connections.Where(c => c.Item1 == state && c.Item2 == input);
            foreach (var connection in connectionToRemove)
            {
                _connections.Remove(connection);
            }

            return this;
        }

        public NetworkBuilder ClearConnectionsFromState(string state)
        {
            var connectionToRemove = _connections.Where(c => c.Item1 == state);
            foreach (var connection in connectionToRemove)
            {
                _connections.Remove(connection);
            }

            return this;
        }

        public NetworkBuilder ClearConnectionsToState(string state, string input)
        {
            var connectionToRemove = _connections.Where(c => c.Item2 == input && c.Item3.Target == state);
            foreach (var connection in connectionToRemove)
            {
                _connections.Remove(connection);
            }

            return this;
        }

        public NetworkBuilder ClearConnectionsToState(string state)
        {
            var connectionToRemove = _connections.Where(c => c.Item3.Target == state);
            foreach (var connection in connectionToRemove)
            {
                _connections.Remove(connection);
            }

            return this;
        }

        public NetworkBuilder ClearConnectionsWithInput(string input)
        {
            var connectionToRemove = _connections.Where(c => c.Item2 == input);
            foreach (var connection in connectionToRemove)
            {
                _connections.Remove(connection);
            }

            return this;
        }

        public StateNetworkResult Build()
        {
            if (string.IsNullOrEmpty(_startState))
            {
                return StateNetworkResult.Fail("Start state was not set.");
            }

            var stateDictionary =
                new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>();

            foreach (var state in _states)
            {
                var inputDictionary = new Dictionary<string, IReadOnlyList<Connection>>();

                var connectionsFromState = _connections.Where(c => c.Item1 == state).GroupBy(c => c.Item2);
                foreach (var connectionGroup in connectionsFromState)
                {
                    var connections = connectionGroup.Select(c => c.Item3).ToImmutableList();
                    inputDictionary.Add(connectionGroup.Key, connections);
                }

                stateDictionary.Add(state, inputDictionary.ToImmutableDictionary());
            }

            var network = new StateNetwork(stateDictionary.ToImmutableDictionary(), _startState);

            Reset();

            return StateNetworkResult.Ok(network, "Success.");
        }

        public void Reset()
        {
            _startState = string.Empty;
            _states.Clear();
            _inputs.Clear();
            _connections.Clear();
        }
    }
}