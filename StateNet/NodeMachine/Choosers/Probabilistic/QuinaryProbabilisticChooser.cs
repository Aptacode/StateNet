using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probabilistic
{
    public class QuinaryProbabilisticChooser : ProbabilisticChooser<QuinaryChoice>
    {
        public QuinaryProbabilisticChooser(Node destinationA,
                                           Node destinationB,
                                           Node destinationC,
                                           Node destinationD,
                                           Node destinationE) : base(new QuinaryOptions(destinationA,
                                                                                        destinationB,
                                                                                        destinationC,
                                                                                        destinationD,
                                                                                        destinationE))
        { }
    }
}
