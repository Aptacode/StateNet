using System;

namespace Aptacode.StateNet.Network.Connections
{
    public sealed class Connection : IEquatable<Connection>
    {
        public Connection(State source, Input input, State target, ConnectionWeight connectionWeight)
        {
            Source = source;
            Input = input;
            Target = target;
            ConnectionWeight = connectionWeight;
        }

        public State Source { get; set; }
        public Input Input { get; set; }
        public State Target { get; set; }
        public ConnectionWeight ConnectionWeight { get; set; }

        public override string ToString() => $"{Source}({Input})->({Target}:{ConnectionWeight})";

        #region Overrides

        public override int GetHashCode() => (Source, Input, Target, Weight: ConnectionWeight).GetHashCode();

        public override bool Equals(object obj) => obj is Connection other && Equals(other);

        public bool Equals(Connection other) =>
            other != null && Source.Equals(other.Source) && Input == other.Input &&
            Target == other.Target &&
            ConnectionWeight.Equals(other.ConnectionWeight);

        #endregion Overrides
    }
}