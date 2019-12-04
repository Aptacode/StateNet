using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;

namespace Aptacode.StateNet.NodeMachine.Choosers
{
    public abstract class NodeChooser
    {
        public abstract Node GetNext();
    }

    public abstract class NodeChooser<TChoices> : NodeChooser
        where TChoices : System.Enum
    {
        public NodeChooser(NodeOptions<TChoices> options) => Options = options;

        public override string ToString() => Options.ToString();

        public NodeOptions<TChoices> Options { get; set; }
    }
}
