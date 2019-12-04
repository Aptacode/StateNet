using System;
using System.Collections.Generic;
using Aptacode.StateNet.NodeMachine.Choices;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class BinaryNode : Node
    {
        private Node DestinationNodeA;
        private Node DestinationNodeB;

        public BinaryNode(string name) : base(name) { }

        public override Node GetNext()
        {
            switch (Chooser.GetChoice())
            {
                case BinaryChoice.Item1:
                    return DestinationNodeA;
                case BinaryChoice.Item2:
                    return DestinationNodeB;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node> { DestinationNodeA, DestinationNodeB };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name}";

        public void Visits(Node destinationNodeA, Node destinationNodeB, IChooser<BinaryChoice> choiceFunction)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            Chooser = choiceFunction;
        }

        public IChooser<BinaryChoice> Chooser { get; set; }
    }
;
}
