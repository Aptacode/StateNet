using System;
using Aptacode.StateNet.Events;

namespace Aptacode.StateNet
{
    public sealed class State : IEquatable<State>
    {
        public State(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public event StateEvent OnVisited;

        public event StateEvent OnUpdateConnections;

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