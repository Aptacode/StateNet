using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class UnaryNode : Node
    {
        public UnaryNode(string name) : this(name, null) { }

        public UnaryNode(string name, Node destinationNode) : base(name) => DestinationNode = destinationNode;

        public override Node GetNext() => DestinationNode ;

        public override IEnumerable<Node> GetNextNodes() => new List<Node> { DestinationNode };

        public override string ToString() => $"{Name}->{DestinationNode.Name}";

        public void Visits(Node destinationNode) => DestinationNode = destinationNode ;

        public Node DestinationNode { get; private set; }
    }
}
