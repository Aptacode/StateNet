using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public delegate void NodeEvent(Node sender);

    public abstract class Node
    {
        public Node(string name) => Name = name;

        public event NodeEvent OnExited;

        public event NodeEvent OnVisited;

        public void Exit() => new TaskFactory().StartNew(() =>
        {
            OnExited?.Invoke(this);
            GetNext().Visit();
        }) ;

        public abstract Node GetNext();

        public abstract IEnumerable<Node> GetNextNodes();

        public void Visit() => new TaskFactory().StartNew(() =>
        {
            OnVisited?.Invoke(this);
        }) ;

        public string Name { get; }
    }
}
