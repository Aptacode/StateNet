using System;
using System.Collections.Generic;
using Aptacode.StateNet.Network.Connections;
using Newtonsoft.Json;

namespace Aptacode.StateNet.Network
{
    public sealed class State : IEquatable<State>
    {
        private readonly List<Connection> _outputConnections = new List<Connection>();

        /// <summary>
        ///     Represents a single state in the network and all of the output connections it has
        /// </summary>
        /// <param name="name">The name of the state</param>
        public State(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     The state name
        /// </summary>
        public string Name { get; set; }

        public override string ToString() => Name;

        public static implicit operator string(State instance) => instance?.Name;

        #region Connections

        [JsonIgnore] public IEnumerable<Connection> Connections => _outputConnections;

        public void Remove(Connection connection)
        {
            _outputConnections.Remove(connection);
        }

        public void Add(Connection connection)
        {
            if (!connection.Source.Equals(this))
            {
                return;
            }

            _outputConnections.Add(connection);
        }

        public bool IsEnd() => _outputConnections.Count == 0;

        #endregion

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj) => obj is State other && Equals(other);

        public bool Equals(State other) => other != null && Name.Equals(other.Name);

        #endregion Equality
    }
}