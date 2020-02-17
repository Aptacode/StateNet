using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.NodeWeights;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aptacode.StateNet
{
    public class StateDistribution
    {
        private readonly Dictionary<State, IConnectionWeight> _distribution = new Dictionary<State, IConnectionWeight>();

        public IConnectionWeight this[State node]
        {
            get
            {
                if (_distribution.ContainsKey(node))
                {
                    return _distribution[node];
                }
                return null;
            }
        }

        public void Invalid() => Clear();

        public void Clear() => _distribution.Clear();

        public void Always(State choice)
        {
            Clear();
            if (choice != null)
            {
                UpdateWeight(choice, 1);
            }
        }

        public void SetDistribution(params (State, int)[] choices)
        {
            Clear();
            UpdateDistribution(choices);
        }

        public void UpdateDistribution(params (State, int)[] choices)
        {
            foreach (var choice in choices)
            {
                UpdateWeight(choice.Item1, choice.Item2);
            }
        }

        public void UpdateWeight(State choice, int weight) => _distribution[choice] = new StaticWeight(weight);

        public void UpdateWeight(State choice, IConnectionWeight weight) => _distribution[choice] = weight;

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

        public IEnumerable<IConnectionWeight> GetWeights() => _distribution.Values;

        public List<KeyValuePair<State, IConnectionWeight>> GetAll() => _distribution.ToList();
    }
}