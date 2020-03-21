using System.Collections.Generic;

namespace Aptacode.StateNet.Connections.Weights
{
    /// <summary>
    ///     A Static weight returns its set weight regardless of the StateHistory
    /// </summary>
    public class StaticWeight : ConnectionWeight
    {
        private int _weight;

        public StaticWeight()
        {
            _weight = 0;
        }

        public StaticWeight(int weight)
        {
            Weight = weight;
        }

        public int Weight
        {
            get => _weight;
            set => _weight = value >= 0 ? value : 0;
        }

        public override string TypeName => nameof(StaticWeight);

        public override int GetWeight(List<(Input, State)> stateHistory)
        {
            return Weight;
        }

        public override int GetHashCode()
        {
            return Weight.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is StaticWeight other && Equals(other);
        }

        public bool Equals(StaticWeight other)
        {
            return other != null && Weight.Equals(other.Weight);
        }


        public override string ToString()
        {
            return $"{TypeName}:{Weight}";
        }
    }
}