using System;
using System.Collections.Generic;
using Aptacode.StateNet.NodeMachine.Choices;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class QuaternaryNode : Node
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;

        public QuaternaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch (Chooser.GetChoice())
            {
                case QuaternaryChoice.Item1:
                    return DestinationNodeA;
                case QuaternaryChoice.Item2:
                    return DestinationNodeB;
                case QuaternaryChoice.Item3:
                    return DestinationNodeC;
                case QuaternaryChoice.Item4:
                    return DestinationNodeD;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node>
        { DestinationNodeA, DestinationNodeB, DestinationNodeC, DestinationNodeD };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name},{DestinationNodeD.Name}";

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           IChooser<QuaternaryChoice> chooser)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            Chooser = chooser;
        }

        public IChooser<QuaternaryChoice> Chooser { get; set; }
    }
}
