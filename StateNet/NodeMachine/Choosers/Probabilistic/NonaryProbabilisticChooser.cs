using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probabilistic
{
    public class NonaryProbabilisticChooser : ProbabilisticChooser<NonaryChoice>
    {
        public NonaryProbabilisticChooser(Node destinationA, Node destinationB, Node destinationC, Node destinationD, Node destinationE, Node destinationF, Node destinationG, Node destinationH, Node destinationI) : base(new NonaryOptions(destinationA, destinationB, destinationC, destinationD, destinationE, destinationF, destinationG, destinationH, destinationI)) { }
    }
}
