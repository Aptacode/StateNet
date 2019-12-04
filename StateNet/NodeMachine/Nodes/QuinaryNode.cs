using System;
using System.Collections.Generic;
using Aptacode.StateNet.NodeMachine.Choices;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class QuinaryNode : Node
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;
        private Node DestinationNodeE;

        public QuinaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch (Chooser.GetChoice())
            {
                case QuinaryChoice.Item1:
                    return DestinationNodeA;
                case QuinaryChoice.Item2:
                    return DestinationNodeB;
                case QuinaryChoice.Item3:
                    return DestinationNodeC;
                case QuinaryChoice.Item4:
                    return DestinationNodeD;
                case QuinaryChoice.Item5:
                    return DestinationNodeE;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node>
        { DestinationNodeA, DestinationNodeB, DestinationNodeC, DestinationNodeD, DestinationNodeE };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name},{DestinationNodeD.Name},{DestinationNodeE.Name}";

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           Node destinationNodeE,
                           IChooser<QuinaryChoice> chooser)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            DestinationNodeE = destinationNodeE;
            Chooser = chooser;
        }

        public IChooser<QuinaryChoice> Chooser { get; set; }
    }
}
