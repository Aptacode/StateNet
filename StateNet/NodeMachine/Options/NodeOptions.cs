using Aptacode.StateNet.NodeMachine.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Options
{
    public abstract class NodeOptions<TChoice>
        where TChoice : System.Enum
    {
        public abstract Node GetNode(TChoice choice);

        public abstract IEnumerable<Node> GetNodes();

        public override string ToString() => string.Join(", ", GetNodes().Select(node => node.Name));
    }
}
