using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Deterministic
{
    public class TernaryDeterministicChooser : DeterministicChooser<TernaryChoice>
    {
        public TernaryDeterministicChooser(Node option1, Node option2, Node option3) : base(new TernaryOptions(option1,
                                                                                                               option2,
                                                                                                               option3),
                                                                                            TernaryChoice.Item1)
        { }
    }
}
