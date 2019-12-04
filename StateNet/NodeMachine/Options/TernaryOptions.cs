using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class TernaryOptions : NodeOptions<TernaryChoice>
    {
        public TernaryOptions(Node destinationA, Node destinationB, Node destinationC)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
            DestinationC = destinationC;
        }

        public override Node GetNode(TernaryChoice choice)
        {
            switch(choice)
            {
                case TernaryChoice.Item1:
                    return DestinationA;
                case TernaryChoice.Item2:
                    return DestinationB;
                case TernaryChoice.Item3:
                    return DestinationC;
                default:
                    throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNodes() => new List<Node> { DestinationA, DestinationB, DestinationC };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }

        public Node DestinationC { get; set; }
    }
}
