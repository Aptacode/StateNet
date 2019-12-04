using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Events;
using System.Threading.Tasks;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class Node
    {
        public Node(string name) => Name = name;

        public event NodeEvent OnExited;

        public event NodeEvent OnVisited;

        public override bool Equals(object obj) => (obj is Node other) && Equals(other);

        public bool Equals(Node other) => Name.Equals(other.Name);

        public void Exit() => new TaskFactory().StartNew(() =>
        {
            OnExited?.Invoke(this);
            Chooser.GetNext().Visit();
        });

        public int GetHashCode(Node obj) => Name.GetHashCode();

        public override string ToString() => $"{Name}->{Chooser}";

        public void Visit() => new TaskFactory().StartNew(() =>
        {
            OnVisited?.Invoke(this);
        });

        public NodeChooser Chooser { get; set; }

        public string Name { get; }
    }
}


