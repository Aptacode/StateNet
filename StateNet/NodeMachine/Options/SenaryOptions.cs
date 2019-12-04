using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class SenaryOptions : NodeOptions<SenaryChoice>
    {
        public SenaryOptions(Node destinationA,
                             Node destinationB,
                             Node destinationC,
                             Node destinationD,
                             Node destinationE,
                             Node destinationF)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
            DestinationC = destinationC;
            DestinationD = destinationD;
            DestinationE = destinationE;
            DestinationF = destinationF;
        }

        public override Node GetNode(SenaryChoice choice)
        {
            switch(choice)
            {
                case SenaryChoice.Item1:
                    return DestinationA;
                case SenaryChoice.Item2:
                    return DestinationB;
                case SenaryChoice.Item3:
                    return DestinationC;
                case SenaryChoice.Item4:
                    return DestinationD;
                case SenaryChoice.Item5:
                    return DestinationE;
                case SenaryChoice.Item6:
                    return DestinationF;
                default:
                    throw new System.Exception();
            }
        }

        public override IEnumerable<Node> GetNodes() => new List<Node>
        { DestinationA, DestinationB, DestinationC, DestinationD, DestinationE, DestinationF };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }

        public Node DestinationC { get; set; }

        public Node DestinationD { get; set; }

        public Node DestinationE { get; set; }

        public Node DestinationF { get; set; }
    }
}
