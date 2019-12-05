using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Deterministic
{
    public class QuaternaryDeterministicChooser : DeterministicChooser<QuaternaryChoice>
    {
        public QuaternaryDeterministicChooser(Node option1, Node option2, Node option3, Node option4) : base(new QuaternaryOptions(option1,
                                                                                                                                   option2,
                                                                                                                                   option3,
                                                                                                                                   option4),
                                                                                                             QuaternaryChoice.Item1)
        { }
    }
}
