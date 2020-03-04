using System;
using Aptacode.StateNet.Events;

namespace Aptacode.StateNet
{
    public sealed class State : IEquatable<State>
    {
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

        /// <summary>
        ///     Raised when the state is entered
        /// </summary>
        public event StateEvent OnVisited;

        /// <summary>
        ///     Raised before the state determines which connection to take
        ///     Use to update probabilities
        /// </summary>
        public event StateEvent OnUpdateConnections;

        /// <summary>
        ///     Raised when the state is exited
        /// </summary>
        public event StateEvent OnExited;

        public override string ToString()
        {
            return Name;
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

        #region Internal

        internal void Visit()
        {
            OnVisited?.Invoke(this);
        }

        internal void Exit()
        {
            OnExited?.Invoke(this);
        }

        internal void UpdateChoosers()
        {
            OnUpdateConnections?.Invoke(this);
        }

        #endregion Internal
    }
}