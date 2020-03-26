using System;

namespace Aptacode.StateNet.Network
{
    public sealed class Input : IEquatable<Input>
    {
        public Input(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     The state name
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator string(Input instance)
        {
            return instance?.Name;
        }

        #region Overrides

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Input other && Equals(other);
        }

        public bool Equals(Input other)
        {
            return other != null && Name.Equals(other.Name);
        }

        #endregion Overrides
    }
}