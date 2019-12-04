using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class BinaryOptions : NodeOptions<BinaryChoice>
    {
        public BinaryOptions(Node destinationA, Node destinationB)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
        }

        public override Node GetNode(BinaryChoice choice)
        {
            switch(choice)
            {
                case BinaryChoice.Item1:
                    return DestinationA;
                case BinaryChoice.Item2:
                    return DestinationB;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNodes() => new List<Node> { DestinationA, DestinationB };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }
    }
}
