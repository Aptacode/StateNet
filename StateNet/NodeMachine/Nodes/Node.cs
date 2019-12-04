using Aptacode.StateNet.NodeMachine.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public abstract class Node
    {
        public Node(string name) => Name = name;

        public event NodeEvent OnExited;

        public event NodeEvent OnVisited;

        public override bool Equals(object obj) => (obj is Node other) && Equals(other);

        public bool Equals(Node other) => Name.Equals(other.Name);

        public void Exit() => new TaskFactory().StartNew(() =>
        {
            OnExited?.Invoke(this);
            GetNext().Visit();
        });

        public int GetHashCode(Node obj) => Name.GetHashCode();

        public abstract Node GetNext();

        public abstract IEnumerable<Node> GetNextNodes();

        public abstract void UpdateReference(Node node);

        public void Visit() => new TaskFactory().StartNew(() =>
        {
            OnVisited?.Invoke(this);
        });

        public string Name { get; }
    }
}
