using Aptacode.StateNet.Events;

namespace Aptacode.StateNet
{
    public sealed class Node : System.IEquatable<Node>
    {
        public Node(string name) => Name = name;

        #region Overrides

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj) => (obj is Node other) && Equals(other);

        public bool Equals(Node other) => Name.Equals(other.Name);

        #endregion Overrides

        #region Internal

        internal void Visit() => OnVisited?.Invoke(this);

        internal void Exit() => OnExited?.Invoke(this);

        internal void UpdateChoosers() => OnUpdateChoosers?.Invoke(this);

        #endregion Internal

        public event NodeEvent OnVisited;

        public event NodeEvent OnUpdateChoosers;

        public event NodeEvent OnExited;

        public override string ToString() => Name;

        public string Name { get; }
    }
}