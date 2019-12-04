using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class EndNode : Node
    {
        public EndNode(string name) : base(name) { }

        public override Node GetNext() => null;

        public override IEnumerable<Node> GetNextNodes() => new List<Node>();

        public override string ToString() => $"{Name}";
    }
}
