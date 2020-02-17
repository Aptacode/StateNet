using Aptacode.StateNet.Interfaces;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeWeights
{
    public class StaticWeight : IConnectionWeight
    {
        public int Weight { get; set; }

        public StaticWeight(int weight) => Weight = weight;

        public int GetWeight(List<State> history) => Weight;
    }
}