using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class NonaryOptions : NodeOptions<NonaryChoice>
    {
        public NonaryOptions(Node destinationA,
                             Node destinationB,
                             Node destinationC,
                             Node destinationD,
                             Node destinationE,
                             Node destinationF,
                             Node destinationG,
                             Node destinationH,
                             Node destinationI)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
            DestinationC = destinationC;
            DestinationD = destinationD;
            DestinationE = destinationE;
            DestinationF = destinationF;
            DestinationG = destinationG;
            DestinationH = destinationH;
            DestinationI = destinationI;
        }

        public override Node GetNode(NonaryChoice choice)
        {
            switch(choice)
            {
                case NonaryChoice.Item1:
                    return DestinationA;
                case NonaryChoice.Item2:
                    return DestinationB;
                case NonaryChoice.Item3:
                    return DestinationC;
                case NonaryChoice.Item4:
                    return DestinationD;
                case NonaryChoice.Item5:
                    return DestinationE;
                case NonaryChoice.Item6:
                    return DestinationF;
                case NonaryChoice.Item7:
                    return DestinationG;
                case NonaryChoice.Item8:
                    return DestinationH;
                case NonaryChoice.Item9:
                    return DestinationI;
                default:
                    throw new System.Exception();
            }
        }

        public override IEnumerable<Node> GetNodes() => new List<Node>
        {
            DestinationA,
            DestinationB,
            DestinationC,
            DestinationD,
            DestinationE,
            DestinationF,
            DestinationG,
            DestinationH,
            DestinationI
        };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }

        public Node DestinationC { get; set; }

        public Node DestinationD { get; set; }

        public Node DestinationE { get; set; }

        public Node DestinationF { get; set; }

        public Node DestinationG { get; set; }

        public Node DestinationH { get; set; }

        public Node DestinationI { get; set; }
    }
}
