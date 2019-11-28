using Aptacode.StateNet.NodeMachine.Choices;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public class BinaryNode : Node
    {
        private Func<IChooser<BinaryChoice>> ChoiceFunction;
        private Node DestinationNodeA;
        private Node DestinationNodeB;

        public BinaryNode(string name) : this(name, null, null, null) { }

        public BinaryNode(string name,
                          Node destinationNodeA,
                          Node destinationNodeB,
                          Func<IChooser<BinaryChoice>> choiceFunction) : base(name)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            ChoiceFunction = choiceFunction;
        }

        public override Node GetNext()
        {
            var choice = ChoiceFunction?.Invoke().GetChoice();
            if(choice.HasValue)
            {
                switch(choice.Value)
                {
                    case BinaryChoice.Item1:
                        return DestinationNodeA;
                    case BinaryChoice.Item2:
                        return DestinationNodeB;
                    default:
                        return null;
                }
            } else
            {
                throw new Exception();
            }
        }

        public override IEnumerable<Node> GetNextNodes() => new List<Node> { DestinationNodeA, DestinationNodeB };

        public override string ToString() => $"{Name}->{DestinationNodeA.Name},{DestinationNodeB.Name}";

        public void Visits(Node destinationNodeA, Node destinationNodeB, Func<IChooser<BinaryChoice>> choiceFunction)
        {
            DestinationNodeA = destinationNodeA;
            DestinationNodeB = destinationNodeB;
            ChoiceFunction = choiceFunction;
        }
    }
;
}
