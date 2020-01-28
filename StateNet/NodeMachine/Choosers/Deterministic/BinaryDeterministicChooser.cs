using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Deterministic
{
    public class BinaryDeterministicChooser : DeterministicChooser<BinaryChoice>
    {
        public BinaryDeterministicChooser(Node option1, Node option2) : base(new BinaryOptions(option1, option2),
                                                                             BinaryChoice.Item1)
        { }
    }
}
