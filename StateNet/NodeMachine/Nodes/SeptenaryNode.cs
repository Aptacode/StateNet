using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class SeptenaryNode : NonDeterministicNode<SeptenaryChoice>
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;
        private Node DestinationNodeE;
        private Node DestinationNodeF;
        private Node DestinationNodeG;

        public SeptenaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch(Chooser.GetChoice())
            {
                case SeptenaryChoice.Item1:
                    return DestinationNodeA;
                case SeptenaryChoice.Item2:
                    return DestinationNodeB;
                case SeptenaryChoice.Item3:
                    return DestinationNodeC;
                case SeptenaryChoice.Item4:
                    return DestinationNodeD;
                case SeptenaryChoice.Item5:
                    return DestinationNodeE;
                case SeptenaryChoice.Item6:
                    return DestinationNodeF;
                case SeptenaryChoice.Item7:
                    return DestinationNodeG;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node>
        {
            DestinationNodeA,
            DestinationNodeB,
            DestinationNodeC,
            DestinationNodeD,
            DestinationNodeE,
            DestinationNodeF,
            DestinationNodeG
        };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name},{DestinationNodeD.Name},{DestinationNodeE.Name},{DestinationNodeF.Name},{DestinationNodeG.Name}";

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           Node destinationNodeE,
                           Node destinationNodeF,
                           Node destinationNodeG,
                           IChooser<SeptenaryChoice> chooser)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            DestinationNodeE = destinationNodeE;
            DestinationNodeF = destinationNodeF;
            DestinationNodeG = destinationNodeG;
            Chooser = chooser;
        }
    }
}
