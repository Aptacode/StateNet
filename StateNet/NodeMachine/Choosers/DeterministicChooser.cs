using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers
{
    public class DeterministicChooser<TChoices> : NodeChooser<TChoices>
        where TChoices : System.Enum
    {
        public DeterministicChooser(NodeOptions<TChoices> choices, TChoices defaultChoice) : base(choices) => Selection =
            defaultChoice;

        public override Node GetNext() => Options.GetNode(Selection) ;

        public TChoices Selection { get; set; }
    }
}
