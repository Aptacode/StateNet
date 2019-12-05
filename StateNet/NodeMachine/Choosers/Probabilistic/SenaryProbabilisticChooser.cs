using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probabilistic
{
    public class SenaryProbabilisticChooser : ProbabilisticChooser<SenaryChoice>
    {
        public SenaryProbabilisticChooser(Node destinationA,
                                          Node destinationB,
                                          Node destinationC,
                                          Node destinationD,
                                          Node destinationE,
                                          Node destinationF) : base(new SenaryOptions(destinationA,
                                                                                      destinationB,
                                                                                      destinationC,
                                                                                      destinationD,
                                                                                      destinationE,
                                                                                      destinationF))
        { }
    }
}
