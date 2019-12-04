using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class OctaryNode : NonDeterministicNode<OctaryChoice>
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;
        private Node DestinationNodeE;
        private Node DestinationNodeF;
        private Node DestinationNodeG;
        private Node DestinationNodeH;

        public OctaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch(Chooser.GetChoice())
            {
                case OctaryChoice.Item1:
                    return DestinationNodeA;
                case OctaryChoice.Item2:
                    return DestinationNodeB;
                case OctaryChoice.Item3:
                    return DestinationNodeC;
                case OctaryChoice.Item4:
                    return DestinationNodeD;
                case OctaryChoice.Item5:
                    return DestinationNodeE;
                case OctaryChoice.Item6:
                    return DestinationNodeF;
                case OctaryChoice.Item7:
                    return DestinationNodeG;
                case OctaryChoice.Item8:
                    return DestinationNodeH;
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

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name},{DestinationNodeD.Name},{DestinationNodeE.Name},{DestinationNodeF.Name},{DestinationNodeG.Name},{DestinationNodeH.Name}";

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
        }

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           Node destinationNodeE,
                           Node destinationNodeF,
                           Node destinationNodeG,
                           Node destinationNodeH,
                           IChooser<OctaryChoice> chooser)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            DestinationNodeE = destinationNodeE;
            DestinationNodeF = destinationNodeF;
            DestinationNodeG = destinationNodeG;
            DestinationNodeH = destinationNodeH;
            Chooser = chooser;
        }
    }
}
