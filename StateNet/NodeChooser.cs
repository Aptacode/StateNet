using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.NodeWeights;

namespace Aptacode.StateNet
{
    public class NodeChooser
    {
        private static readonly Random RandomGenerator = new Random(new Guid().GetHashCode());

        private readonly Dictionary<Node, INodeWeight> _distribution = new Dictionary<Node, INodeWeight>();

        internal Node Next(List<Node> history)
        {
            if (TotalWeight(history) == 0)
            {
                return null;
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight(history) + 1);

            var weightSum = 0;
            foreach (var keyValue in _distribution)
            {
                weightSum += keyValue.Value.GetWeight(history);
                if (weightSum >= randomChoice)
                {
                    return keyValue.Key;
                }
            }

            throw new Exception();
        }

        public int GetWeight(Node choice, List<Node> history)
        {
            if (_distribution.ContainsKey(choice))
            {
                return _distribution[choice].GetWeight(history);
            }
            else
            {
                return 0;
            }
        }

        public void Always(Node choice)
        {
            Clear();
            if (choice != null)
            {
                UpdateWeight(choice, 1);
            }
        }
        public void Invalid() => Clear();
        public void Clear() => _distribution.Clear();

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

        public int TotalWeight(List<Node> history) => _distribution.Values.Select(f => f.GetWeight(history)).Sum();

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
    }
}
