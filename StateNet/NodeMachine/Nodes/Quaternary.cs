using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class QuaternaryNode : Node
    {
        private Func<IChooser<QuaternaryChoice>> ChoiceFunction;
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;
        private Node DestinationNodeD;

        public QuaternaryNode(string name) : this(name, null, null, null, null, null) { }

        public QuaternaryNode(string name,
                           Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           Func<IChooser<QuaternaryChoice>> choiceFunction) : base(name)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            ChoiceFunction = choiceFunction;
        }

        public override Node GetNext()
        {
            var choice = ChoiceFunction?.Invoke().GetChoice();
            if(choice.HasValue)
            {
                switch(choice.Value)
                {
                    case QuaternaryChoice.Item1:
                        return DestinationNodeA;
                    case QuaternaryChoice.Item2:
                        return DestinationNodeB;
                    case QuaternaryChoice.Item3:
                        return DestinationNodeC;
                    case QuaternaryChoice.Item4:
                        return DestinationNodeD;
                    default:
                        return null;
                }
            } else
            {
                throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node>
        { DestinationNodeA, DestinationNodeB, DestinationNodeC };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name},{DestinationNodeC.Name}";

        public void Visits(Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Node destinationNodeD,
                           Func<IChooser<QuaternaryChoice>> choiceFunction)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            DestinationNodeD = destinationNodeD;
            ChoiceFunction = choiceFunction;
        }
    }
}
