using Aptacode.StateNet.Events;

namespace Aptacode.StateNet
{
    public class Node
    {
        private readonly NodeChooserCollection _chooserCollection;

        public Node(string name)
        {
            _chooserCollection = new NodeChooserCollection();
            Name = name;
        }

        #region Overrides
        public int GetHashCode(Node obj) => Name.GetHashCode();
        public override bool Equals(object obj) => (obj is Node other) && Equals(other);
        public bool Equals(Node other) => Name.Equals(other.Name);
        public override string ToString() => $"{Name}{(_chooserCollection.HasValidChoice ? ":" + _chooserCollection : "")}";
        #endregion

        #region Internal
        internal void Visit() => OnVisited?.Invoke(this);
        internal void Exit() => OnExited?.Invoke(this);
        internal void UpdateChoosers() => OnUpdateChoosers?.Invoke(this);
        #endregion


        public event NodeEvent OnVisited;
        public event NodeEvent OnUpdateChoosers;
        public event NodeEvent OnExited;

        public NodeChooser this[string key]
        {
            get
            {
                return _chooserCollection[key];
            }

            set
            {
                _chooserCollection[key] = value;
            }
        }

        public Node Next(string actionName) => _chooserCollection.Next(actionName);

        public bool IsEndNode => !_chooserCollection.HasValidChoice;

        public string Name { get; }
    }
}


