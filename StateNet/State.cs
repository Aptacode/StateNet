using System;
using System.Collections.Generic;
using Aptacode.StateNet.Connections;

namespace Aptacode.StateNet
{
    public sealed class State : IEquatable<State>
    {
        private readonly List<Connection> _connections = new List<Connection>();

        /// <summary>
        ///     Represents a single state in the network
        /// </summary>
        /// <param name="name">The name of the state</param>
        public State(string name)
        {
            Name = name;
        }   

        /// <summary>
        ///     The state name
        /// </summary>
        public string Name { get; }

        public static State Invalid { get; } = new State(string.Empty);

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator string(State instance)
        {
            return instance?.Name;
        }

        public IEnumerable<Connection> GetConnections()
        {
            return _connections;
        }

        public void Remove(Connection connection)
        {
            _connections.Remove(connection);
        }

        public void Add(Connection connection)
        {
            _connections.Add(connection);
        }

        public bool IsEnd()
        {
            return _connections.Count == 0;
        }

        #region Overrides

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is State other && Equals(other);
        }

        public bool Equals(State other)
        {
            return other != null && Name.Equals(other.Name);
        }

        #endregion Overrides
    }
}