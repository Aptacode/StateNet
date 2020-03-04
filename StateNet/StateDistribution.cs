using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aptacode.StateNet.ConnectionWeight;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{
    public class StateDistribution
    {
        private readonly Dictionary<State, IConnectionWeight>
            _distribution = new Dictionary<State, IConnectionWeight>();

        public IConnectionWeight this[State node] => _distribution.ContainsKey(node) ? _distribution[node] : null;

        public bool IsInvalid => _distribution.Count == 0;

        public void Invalidate()
        {
            Clear();
        }

        public void Clear()
        {
            _distribution.Clear();
        }

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
            foreach (var (state, item2) in choices)
            {
                UpdateWeight(state, item2);
            }
        }

        public void UpdateWeight(State choice, int weight)
        {
            _distribution[choice] = new StaticWeight(weight);
        }

        public void UpdateWeight(State choice, IConnectionWeight weight)
        {
            _distribution[choice] = weight;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var pairs = _distribution.ToList();
            if (pairs.Count == 0)
            {
                return stringBuilder.ToString();
            }

            stringBuilder
                .Append('(')
                .Append(pairs[0].Key.Name)
                .Append(':')
                .Append(pairs[0].Value)
                .Append(')');
            for (var i = 1; i < pairs.Count; i++)
            {
                stringBuilder
                    .Append(",(")
                    .Append(pairs[i].Key.Name)
                    .Append(':')
                    .Append(pairs[i].Value)
                    .Append(')');
            }

            return stringBuilder.ToString();
        }

        public IEnumerable<IConnectionWeight> GetWeights()
        {
            return _distribution.Values;
        }

        public List<KeyValuePair<State, IConnectionWeight>> GetAll()
        {
            return _distribution.ToList();
        }
    }
}