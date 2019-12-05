using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probabilistic
{
    public class TernaryProbabilisticChooser : ProbabilisticChooser<TernaryChoice>
    {
        public TernaryProbabilisticChooser(Node destinationA, Node destinationB, Node destinationC) : base(new TernaryOptions(destinationA,
                                                                                                                              destinationB,
                                                                                                                              destinationC))
        { }
    }
}
