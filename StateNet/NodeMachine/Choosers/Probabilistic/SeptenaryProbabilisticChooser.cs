using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probabilistic
{
    public class SeptenaryProbabilisticChooser : ProbabilisticChooser<SeptenaryChoice>
    {
        public SeptenaryProbabilisticChooser(Node destinationA, Node destinationB, Node destinationC, Node destinationD, Node destinationE, Node destinationF, Node destinationG) : base(new SeptenaryOptions(destinationA, destinationB, destinationC, destinationD, destinationE, destinationF, destinationG)) { }
    }
}
