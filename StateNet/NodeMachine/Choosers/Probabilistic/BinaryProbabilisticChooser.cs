using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probabilistic
{
    public class BinaryProbabilisticChooser : ProbabilisticChooser<BinaryChoice>
    {
        public BinaryProbabilisticChooser(Node destinationA, Node destinationB) : base(new BinaryOptions(destinationA,
                                                                                                         destinationB))
        { }
    }
}
