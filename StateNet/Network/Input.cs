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
        ///     The Input name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the name of the input
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
        /// <summary>
        /// Allows an input to be used interchangeably as a string
        /// </summary>
        /// <param name="instance"></param>

        public static implicit operator string(Input instance) => instance?.Name;

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj) => obj is Input other && Equals(other);

        public bool Equals(Input other) => other != null && Name.Equals(other.Name);

        #endregion Equality
    }
}