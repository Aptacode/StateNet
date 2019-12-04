using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class QuaternaryNode : NonDeterministicNode<QuaternaryChoice>
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;

        public QuaternaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch(Chooser.GetChoice())
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

        public override void UpdateReference(Node node)
        {
            if(DestinationNodeA?.Equals(node) == true)
            {
                DestinationNodeA = node;
            }
            if(DestinationNodeB?.Equals(node) == true)
            {
                DestinationNodeB = node;
            }
            if(DestinationNodeC?.Equals(node) == true)
            {
                DestinationNodeC = node;
            }
            if(DestinationNodeD?.Equals(node) == true)
            {
                DestinationNodeD = node;
            }
        }

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
    }
}
