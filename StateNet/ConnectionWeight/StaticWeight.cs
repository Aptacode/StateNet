using System.Collections.Generic;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.ConnectionWeight
{
    public class StaticWeight : IConnectionWeight
    {
        public StaticWeight(int weight)
        {
            Weight = weight;
        }

        public int Weight { get; set; }

        public int GetWeight(List<State> history)
        {
            return Weight;
        }
    }
}