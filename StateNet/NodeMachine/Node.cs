using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class Node
    {
        public Node(string name)
        {
            Name = name;
            Choosers = new NodeChooserCollection();
        }

        public event NodeEvent OnExited;

        public event NodeEvent OnUpdateChoosers;

        public event NodeEvent OnVisited;

        public override bool Equals(object obj) => (obj is Node other) && Equals(other);

        public bool Equals(Node other) => Name.Equals(other.Name);

        public void Exit() => OnExited?.Invoke(this);

        internal void UpdateChoosers() => OnUpdateChoosers?.Invoke(this);

        public int GetHashCode(Node obj) => Name.GetHashCode();

        public override string ToString() => $"{Name}{(Choosers.HasValidChoice ? ":" + Choosers : "")}";

        public void Visit() => OnVisited?.Invoke(this);

        private readonly NodeChooserCollection Choosers;

        public NodeChooser this[string key]
        {
            get
            {
                return Choosers[key];
            }

            set
            {
                Choosers[key] = value;
            }
        }

        public Node Next(string actionName) => Choosers.Next(actionName);

        public bool IsEndNode => !Choosers.HasValidChoice;


        public string Name { get; }
    }
}


