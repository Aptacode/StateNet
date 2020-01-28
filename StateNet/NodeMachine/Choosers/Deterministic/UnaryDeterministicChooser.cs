using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Deterministic
{
    public class UnaryDeterministicChooser : DeterministicChooser<UnaryChoice>
    {
        public UnaryDeterministicChooser(Node destination) : base(new UnaryOptions(destination), UnaryChoice.Item1) { }
    }
}
