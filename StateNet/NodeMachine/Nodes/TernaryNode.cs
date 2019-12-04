using System;
using System.Collections.Generic;
using Aptacode.StateNet.NodeMachine.Choices;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class TernaryNode : Node
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;

        public TernaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch (Chooser.GetChoice())
            {
                case TernaryChoice.Item1:
                    return DestinationNodeA;
                case TernaryChoice.Item2:
                    return DestinationNodeB;
                case TernaryChoice.Item3:
                    return DestinationNodeC;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node>
        { DestinationNodeA, DestinationNodeB, DestinationNodeC };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name}";

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           IChooser<TernaryChoice> chooser)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            Chooser = chooser;
        }

        public IChooser<TernaryChoice> Chooser { get; set; }
    }
}
