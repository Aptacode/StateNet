using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.NodeWeights;

namespace Aptacode.StateNet.Connections
{
    public class NodeConnections
    {
        private readonly Dictionary<Node, INodeWeight> _distribution = new Dictionary<Node, INodeWeight>();

        public INodeWeight this[Node node]
        {
            get
            {
                if (_distribution.ContainsKey(node))
                {
                    return _distribution[node];
                }
                return null;
            }
        }
        public void Invalid() => Clear();
        public void Clear() => _distribution.Clear();
        public void Always(Node choice)
        {
            Clear();
            if (choice != null)
            {
                UpdateWeight(choice, 1);
            }
        }

        public void SetDistribution(params (Node, int)[] choices)
        {
            Clear();
            UpdateDistribution(choices);
        }

        public void UpdateDistribution(params (Node, int)[] choices)
        {
            foreach (var choice in choices)
            {
                UpdateWeight(choice.Item1, choice.Item2);
            }
        }
        public void UpdateWeight(Node choice, int weight) => _distribution[choice] = new StaticNodeWeight(weight);

        public void UpdateWeight(Node choice, INodeWeight weight) => _distribution[choice] = weight;

        public bool IsInvalid => _distribution.Count == 0;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var pairs = _distribution.ToList();
            if (pairs.Count > 0)
            {
                stringBuilder.Append($"({pairs[0].Key.Name}:{pairs[0].Value})");
                for (var i = 1; i < pairs.Count; i++)
                {
                    stringBuilder.Append($",({pairs[i].Key.Name}:{pairs[i].Value})");
                }
            }

            return stringBuilder.ToString();
        }

        public IEnumerable<INodeWeight> GetWeights() => _distribution.Values;
        public List<KeyValuePair<Node,INodeWeight>> GetAll() => _distribution.ToList();

    }
}
