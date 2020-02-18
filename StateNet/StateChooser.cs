using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet
{
    public class StateChooser
    {
        private static readonly Random RandomGenerator = new Random();
        private readonly List<State> _history;

        public StateChooser(List<State> history)
        {
            _history = history;
        }

        internal State Choose(StateDistribution connections)
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

        public int TotalWeight(StateDistribution connections)
        {
            return connections.GetWeights().Select(f => f.GetWeight(_history)).Sum();
        }
    }
}