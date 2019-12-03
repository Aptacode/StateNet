using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class SenaryNode : Node
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;
        private Node DestinationNodeE;
        private Node DestinationNodeF;

        public SenaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch(Chooser.GetChoice())
            {
                case SenaryChoice.Item1:
                    return DestinationNodeA;
                case SenaryChoice.Item2:
                    return DestinationNodeB;
                case SenaryChoice.Item3:
                    return DestinationNodeC;
                case SenaryChoice.Item4:
                    return DestinationNodeD;
                case SenaryChoice.Item5:
                    return DestinationNodeE;
                case SenaryChoice.Item6:
                    return DestinationNodeF;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node>
        { DestinationNodeA, DestinationNodeB, DestinationNodeC, DestinationNodeD, DestinationNodeE, DestinationNodeF };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name},{DestinationNodeD.Name},{DestinationNodeE.Name},{DestinationNodeF.Name}";

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           Node destinationNodeE,
                           Node destinationNodeF,
                           IChooser<SenaryChoice> chooser)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            DestinationNodeE = destinationNodeE;
            DestinationNodeF = destinationNodeF;
            Chooser = chooser;
        }

        public IChooser<SenaryChoice> Chooser { get; set; }
    }
}
