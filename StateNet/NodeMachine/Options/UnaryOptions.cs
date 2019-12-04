using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class UnaryOptions : NodeOptions<UnaryChoice>
    {
        public UnaryOptions(Node destination) => Destination = destination;

        public override Node GetNode(UnaryChoice choice) => Destination;

        public override IEnumerable<Node> GetNodes() => new List<Node> { Destination };

        public Node Destination { get; set; }
    }
}
