using Aptacode.StateNet.Interfaces;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeWeights
{
    public class StaticNodeWeight : INodeWeight
    {
        public int Weight { get; set; }

        public StaticNodeWeight(int weight) => Weight = weight;

        public int GetWeight(List<Node> history) => Weight;
    }
}