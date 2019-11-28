using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class TernaryNode : Node
    {
        private Func<IChooser<TernaryChoice>> ChoiceFunction;
        private Node DestinationNodeA;
        private Node DestinationNodeB;
        private Node DestinationNodeC;

        public TernaryNode(string name) : this(name, null, null, null, null) { }

        public TernaryNode(string name,
                           Node destinationNodeA,
                           Node destinationNodeB,
                           Node destinationNodeC,
                           Func<IChooser<TernaryChoice>> choiceFunction) : base(name)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            ChoiceFunction = choiceFunction;
        }

        public override Node GetNext()
        {
            var choice = ChoiceFunction?.Invoke().GetChoice();
            if(choice.HasValue)
            {
                switch(choice.Value)
                {
                    case TernaryChoice.Item1:
                        return DestinationNodeA;
                    case TernaryChoice.Item2:
                        return DestinationNodeB;
                    case TernaryChoice.Item3:
                        return DestinationNodeC;
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
                           Func<IChooser<TernaryChoice>> choiceFunction)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            DestinationNodeC = destinationNodeC;
            ChoiceFunction = choiceFunction;
        }
    }
}
