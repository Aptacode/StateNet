using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class QuaternaryOptions : NodeOptions<QuaternaryChoice>
    {
        public QuaternaryOptions(Node destinationA, Node destinationB, Node destinationC, Node destinationD)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
            DestinationC = destinationC;
            DestinationD = destinationD;
        }

        public override Node GetNode(QuaternaryChoice choice)
        {
            switch(choice)
            {
                case QuaternaryChoice.Item1:
                    return DestinationA;
                case QuaternaryChoice.Item2:
                    return DestinationB;
                case QuaternaryChoice.Item3:
                    return DestinationC;
                case QuaternaryChoice.Item4:
                    return DestinationD;
                default:
                    throw new System.Exception();
            }
        }

        public override IEnumerable<Node> GetNodes() => new List<Node>
        { DestinationA, DestinationB, DestinationC, DestinationD };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }

        public Node DestinationC { get; set; }

        public Node DestinationD { get; set; }
    }
}
