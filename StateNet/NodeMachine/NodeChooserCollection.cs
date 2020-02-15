using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptacode.StateNet.NodeMachine.Nodes;

namespace Aptacode.StateNet.NodeMachine.Choosers
{
    public class NodeChooserCollection
    {
        private readonly Dictionary<string, NodeChooser> Choosers = new Dictionary<string, NodeChooser>();
        public NodeChooser this[string key]
        {
            get
            {
                
                if (Choosers.TryGetValue(key, out var value))
                {
                    return value;
                }
                else
                {
                    var nodeChooser = new NodeChooser();
                    Choosers.Add(key, nodeChooser);
                    return nodeChooser;
                }
            }

            set
            {
                Choosers[key] = value;
            }
        }

        public Node Next(string actionName)
        {
            if(Choosers.TryGetValue(actionName, out var chooser))
            {
                return chooser?.Next();
            }
            else
            {
                return null;
            }
        }

        public bool HasValidChoice => Choosers.Count(c => c.Value.TotalWeight > 0) > 0;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var pairs = Choosers.ToList();
            if (pairs.Count > 0)
            {
                stringBuilder.Append($"({pairs[0].Key}->{pairs[0].Value})");
                for (int i = 1; i < pairs.Count; i++)
                {
                    stringBuilder.Append($",({pairs[i].Key}->{pairs[i].Value})");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
