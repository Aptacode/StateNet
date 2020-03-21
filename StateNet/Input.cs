using System;
using Aptacode.StateNet.Events;

namespace Aptacode.StateNet
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
        public string Name { get; }

        public static Input Empty { get; } = new Input(string.Empty);

        public event InputEvent OnApplied;

        public override string ToString()
        {
            return Name;
        }

        #region Internal

        internal void Apply()
        {
            OnApplied?.Invoke(this);
        }

        #endregion Internal

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