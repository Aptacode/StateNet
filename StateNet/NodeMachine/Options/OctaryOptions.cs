using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public class OctaryOptions : NodeOptions<OctaryChoice>
    {
        public OctaryOptions(Node destinationA,
                             Node destinationB,
                             Node destinationC,
                             Node destinationD,
                             Node destinationE,
                             Node destinationF,
                             Node destinationG,
                             Node destinationH)
        {
            DestinationA = destinationA;
            DestinationB = destinationB;
            DestinationC = destinationC;
            DestinationD = destinationD;
            DestinationE = destinationE;
            DestinationF = destinationF;
            DestinationH = destinationH;
        }

        public override Node GetNode(OctaryChoice choice)
        {
            switch(choice)
            {
                case OctaryChoice.Item1:
                    return DestinationA;
                case OctaryChoice.Item2:
                    return DestinationB;
                case OctaryChoice.Item3:
                    return DestinationC;
                case OctaryChoice.Item4:
                    return DestinationD;
                case OctaryChoice.Item5:
                    return DestinationE;
                case OctaryChoice.Item6:
                    return DestinationF;
                case OctaryChoice.Item7:
                    return DestinationG;
                case OctaryChoice.Item8:
                    return DestinationH;
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
            DestinationH
        };

        public Node DestinationA { get; set; }

        public Node DestinationB { get; set; }

        public Node DestinationC { get; set; }

        public Node DestinationD { get; set; }

        public Node DestinationE { get; set; }

        public Node DestinationF { get; set; }

        public Node DestinationG { get; set; }

        public Node DestinationH { get; set; }
    }
}
