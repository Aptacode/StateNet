using System.Collections.Generic;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.ConnectionWeight
{
    public class StaticWeight : IConnectionWeight
    {
        private int _weight;

        public StaticWeight(int weight)
        {
            Weight = weight;
        }

        public int Weight
        {
            get => _weight;
            set => _weight = value >= 0 ? value : 0;
        }

        public int GetConnectionWeight(List<State> stateHistory)
        {
            return Weight;
        }
    }
}