using System;

namespace Aptacode.StateNet.Network
{
    public sealed class Connection : IEquatable<Connection>
    {
        public Connection(State fromState, Input input, State toState, ConnectionWeight connectionWeight)
        {
            From = fromState;
            Input = input;
            To = toState;
            ConnectionWeight = connectionWeight;
        }

        public State From { get; set; }
        public Input Input { get; set; }
        public State To { get; set; }
        public ConnectionWeight ConnectionWeight { get; set; }

        public override string ToString()
        {
            return $"{From}({Input})->({To}:{ConnectionWeight})";
        }

        #region Overrides

        public override int GetHashCode()
        {
            return (From, Input, To, Weight: ConnectionWeight).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Connection other && Equals(other);
        }

        public bool Equals(Connection other)
        {
            return other != null && From.Equals(other.From) && Input.Equals(other.Input) && To.Equals(other.To) &&
                   ConnectionWeight.Equals(other.ConnectionWeight);
        }

        #endregion Overrides
    }
}