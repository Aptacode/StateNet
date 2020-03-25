﻿using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Network
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

        internal void Remove(Connection connection)
        {
            _connections.Remove(connection);
        }

        internal void Add(Connection connection)
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