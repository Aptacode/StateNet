using System;

namespace Aptacode.StateNet.Connections
{
    public sealed class Connection : IEquatable<Connection>
    {
        public Connection(State fromState, Input input, State toState, ConnectionWeight weight)
        {
            From = fromState;
            Input = input;
            To = toState;
            Weight = weight;
        }

        public State From { get; set; }
        public Input Input { get; set; }
        public State To { get; set; }
        public ConnectionWeight Weight { get; set; }
        public override string ToString()
        {
            return $"{From}({Input})->({To}:{Weight})";
        }

        #region Overrides

        public override int GetHashCode()
        {
            return (From, Input, To, Weight).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Connection other && Equals(other);
        }

        public bool Equals(Connection other)
        {
            return other != null && From.Equals(other.From) && Input.Equals(other.Input) && To.Equals(other.To) &&
                   Weight.Equals(other.Weight);
        }

        #endregion Overrides
    }
}