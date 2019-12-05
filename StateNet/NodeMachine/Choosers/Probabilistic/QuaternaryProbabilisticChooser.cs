using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probabilistic
{
    public class QuaternaryProbabilisticChooser : ProbabilisticChooser<QuaternaryChoice>
    {
        public QuaternaryProbabilisticChooser(Node destinationA,
                                              Node destinationB,
                                              Node destinationC,
                                              Node destinationD) : base(new QuaternaryOptions(destinationA,
                                                                                              destinationB,
                                                                                              destinationC,
                                                                                              destinationD))
        { }
    }
}
