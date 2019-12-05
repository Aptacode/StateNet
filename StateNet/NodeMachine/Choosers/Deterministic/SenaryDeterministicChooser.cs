using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Deterministic
{
    public class SenaryDeterministicChooser : DeterministicChooser<SenaryChoice>
    {
        public SenaryDeterministicChooser(Node option1, Node option2, Node option3, Node option4, Node option5, Node option6) : base(new SenaryOptions(option1, option2, option3, option4, option5, option6), SenaryChoice.Item1) { }
    }
}
