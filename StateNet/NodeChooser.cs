using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Connections;

namespace Aptacode.StateNet
{
    public class NodeChooser
    {
        private static readonly Random RandomGenerator = new Random(new Guid().GetHashCode());
        private readonly List<Node> _history;

        public NodeChooser(List<Node> history)
        {
            _history = history;
        }

        public NodeChooser()
        {
        }

        internal Node Next(NodeConnections connections)
        {
            var totalWeight = TotalWeight(connections);

            if (totalWeight == 0)
            {
                return null;
            }

            var randomChoice = RandomGenerator.Next(1, totalWeight + 1);

            var weightSum = 0;
            foreach (var keyValue in connections.GetAll())
            {
                weightSum += keyValue.Value.GetWeight(_history);
                if (weightSum >= randomChoice)
                {
                    return keyValue.Key;
                }
            }

            return null;
        }

        public int TotalWeight(NodeConnections connections) => connections.GetWeights().Select(f => f.GetWeight(_history)).Sum();

    }
}
