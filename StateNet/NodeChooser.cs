using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aptacode.StateNet
{
    public class NodeChooser
    {
        private static readonly Random RandomGenerator = new Random(new Guid().GetHashCode());

        private readonly Dictionary<Node, int> _distribution = new Dictionary<Node, int>();

        public NodeChooser() => TotalWeight = 0;

        internal Node Next()
        {
            if (TotalWeight == 0)
            {
                return null;
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);

            var weightSum = 0;
            foreach (var keyValue in _distribution)
            {
                weightSum += keyValue.Value;
                if (weightSum >= randomChoice)
                {
                    return keyValue.Key;
                }
            }

            throw new Exception();
        }

        public int GetWeight(Node choice)
        {
            if (_distribution.ContainsKey(choice))
            {
                return _distribution[choice];
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
        public void Clear()
        {
            TotalWeight = 0;
            _distribution.Clear();
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
        public void UpdateWeight(Node choice, int weight)
        {
            TotalWeight += weight - GetWeight(choice);
            _distribution[choice] = weight;
        }

        public int TotalWeight { get; private set; }

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
