using Aptacode.StateNet.Events;

namespace Aptacode.StateNet
{
    public sealed class State : System.IEquatable<State>
    {
        public State(string name) => Name = name;

        #region Overrides

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj) => (obj is State other) && Equals(other);

        public bool Equals(State other) => Name.Equals(other.Name);

        #endregion Overrides

        #region Internal

        internal void Visit() => OnVisited?.Invoke(this);

        internal void Exit() => OnExited?.Invoke(this);

        internal void UpdateChoosers() => OnUpdateConnections?.Invoke(this);

        #endregion Internal

        public event StateEvent OnVisited;

        public event StateEvent OnUpdateConnections;

        public event StateEvent OnExited;

        public override string ToString() => Name;

        public string Name { get; }
    }
}