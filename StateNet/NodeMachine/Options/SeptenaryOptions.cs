using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class SeptenaryOptions : NodeOptions<SeptenaryChoice>
    {
        public SeptenaryOptions(Node destinationA,
                                Node destinationB,
                                Node destinationC,
                                Node destinationD,
                                Node destinationE,
                                Node destinationF,
                                Node destinationG)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
            DestinationC = destinationC;
            DestinationD = destinationD;
            DestinationE = destinationE;
            DestinationF = destinationF;
            DestinationG = destinationG;
        }

        public override Node GetNode(SeptenaryChoice choice)
        {
            switch(choice)
            {
                case SeptenaryChoice.Item1:
                    return DestinationA;
                case SeptenaryChoice.Item2:
                    return DestinationB;
                case SeptenaryChoice.Item3:
                    return DestinationC;
                case SeptenaryChoice.Item4:
                    return DestinationD;
                case SeptenaryChoice.Item5:
                    return DestinationE;
                case SeptenaryChoice.Item6:
                    return DestinationF;
                case SeptenaryChoice.Item7:
                    return DestinationG;
                default:
                    throw new System.Exception();
            }
        }

        public override IEnumerable<Node> GetNodes() => new List<Node>
        { DestinationA, DestinationB, DestinationC, DestinationD, DestinationE, DestinationF, DestinationG };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }

        public Node DestinationC { get; set; }

        public Node DestinationD { get; set; }

        public Node DestinationE { get; set; }

        public Node DestinationF { get; set; }

        public Node DestinationG { get; set; }
    }
}
