using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Connections.Weights
{
    public abstract class ConnectionWeight : IEquatable<ConnectionWeight>
    {
        public abstract string TypeName { get; }

        public bool Equals(ConnectionWeight other)
        {
            return Equals((object) other);
        }

        public abstract int GetWeight(List<State> stateHistory);
        public abstract override string ToString();

        public abstract int GetHashCode();
        public abstract bool Equals(object obj);
    }
}