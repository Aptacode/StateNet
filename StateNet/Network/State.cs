using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Network
{
    public sealed class State : IEquatable<State>
    {
        private readonly List<Connection> _outputConnections = new List<Connection>();

        /// <summary>
        /// Represents a single state in the network and all of the output connections it has
        /// </summary>
        /// <param name="name">The name of the state</param>
        public State(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The state name
        /// </summary>
        public string Name { get; internal set; }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator string(State instance)
        {
            return instance?.Name;
        }

        #region Connections

        public IEnumerable<Connection> GetOutputConnections()
        {
            return _outputConnections;
        }

        internal void Remove(Connection connection)
        {
            _outputConnections.Remove(connection);
        }

        internal void Add(Connection connection)
        {
            _outputConnections.Add(connection);
        }

        public bool IsEnd()
        {
            return _outputConnections.Count == 0;
        }

        #endregion

        #region Equality

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

        #endregion Equality
    }
}