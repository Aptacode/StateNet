using System;

namespace Aptacode.StateNet.Network
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

        public override string ToString()
        {
            return $"{Source}({Input})->({Target}:{ConnectionWeight})";
        }

        #region Overrides

        public override int GetHashCode()
        {
            return (Source, Input, Target, Weight: ConnectionWeight).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Connection other && Equals(other);
        }

        public bool Equals(Connection other)
        {
            return other != null && Source.Equals(other.Source) && Input.Equals(other.Input) &&
                   Target.Equals(other.Target) &&
                   ConnectionWeight.Equals(other.ConnectionWeight);
        }

        #endregion Overrides
    }
}