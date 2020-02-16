using System.Collections.Generic;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.NodeWeights
{
    public class StaticNodeWeight : INodeWeight
    {
        public int Weight { get; set; }

        public StaticNodeWeight(int weight) => Weight = weight;

        public int GetWeight(List<Node> history) => Weight;
    }
}
