using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class NonaryNode : NonDeterministicNode<NonaryChoice>
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;
        private Node DestinationNodeE;
        private Node DestinationNodeF;
        private Node DestinationNodeG;
        private Node DestinationNodeH;
        private Node DestinationNodeI;

        public NonaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch(Chooser.GetChoice())
            {
                case NonaryChoice.Item1:
                    return DestinationNodeA;
                case NonaryChoice.Item2:
                    return DestinationNodeB;
                case NonaryChoice.Item3:
                    return DestinationNodeC;
                case NonaryChoice.Item4:
                    return DestinationNodeD;
                case NonaryChoice.Item5:
                    return DestinationNodeE;
                case NonaryChoice.Item6:
                    return DestinationNodeF;
                case NonaryChoice.Item7:
                    return DestinationNodeG;
                case NonaryChoice.Item8:
                    return DestinationNodeH;
                case NonaryChoice.Item9:
                    return DestinationNodeI;
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

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name},{DestinationNodeD.Name},{DestinationNodeE.Name},{DestinationNodeF.Name},{DestinationNodeG.Name},{DestinationNodeH.Name},{DestinationNodeI.Name}";

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
            if(DestinationNodeE?.Equals(node) == true)
            {
                DestinationNodeE = node;
            }
            if(DestinationNodeF?.Equals(node) == true)
            {
                DestinationNodeF = node;
            }
            if(DestinationNodeG?.Equals(node) == true)
            {
                DestinationNodeG = node;
            }
            if(DestinationNodeH?.Equals(node) == true)
            {
                DestinationNodeH = node;
            }
            if(DestinationNodeI?.Equals(node) == true)
            {
                DestinationNodeI = node;
            }
        }

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           Node destinationNodeE,
                           Node destinationNodeF,
                           Node destinationNodeG,
                           Node destinationNodeH,
                           Node destinationNodeI,
                           IChooser<NonaryChoice> chooser)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            DestinationNodeE = destinationNodeE;
            DestinationNodeF = destinationNodeF;
            DestinationNodeG = destinationNodeG;
            DestinationNodeH = destinationNodeH;
            DestinationNodeI = destinationNodeI;
            Chooser = chooser;
        }
    }
}
