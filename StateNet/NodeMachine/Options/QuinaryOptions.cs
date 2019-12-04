using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class QuinaryOptions : NodeOptions<QuinaryChoice>
    {
        public QuinaryOptions(Node destinationA,
                              Node destinationB,
                              Node destinationC,
                              Node destinationD,
                              Node destinationE)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
            DestinationC = destinationC;
            DestinationD = destinationD;
            DestinationE = destinationE;
        }

        public override Node GetNode(QuinaryChoice choice)
        {
            switch(choice)
            {
                case QuinaryChoice.Item1:
                    return DestinationA;
                case QuinaryChoice.Item2:
                    return DestinationB;
                case QuinaryChoice.Item3:
                    return DestinationC;
                case QuinaryChoice.Item4:
                    return DestinationD;
                case QuinaryChoice.Item5:
                    return DestinationE;
                default:
                    throw new System.Exception();
            }
        }

        public override IEnumerable<Node> GetNodes() => new List<Node>
        { DestinationA, DestinationB, DestinationC, DestinationD, DestinationE };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }

        public Node DestinationC { get; set; }

        public Node DestinationD { get; set; }

        public Node DestinationE { get; set; }
    }
}
