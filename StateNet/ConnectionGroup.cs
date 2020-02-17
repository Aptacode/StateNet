using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet
{
    public class ConnectionGroup
    {
        private readonly Dictionary<string, StateDistribution> _distributionDictionary = new Dictionary<string, StateDistribution>();

        public StateDistribution this[string action]
        {
            get
            {
                if (!_distributionDictionary.TryGetValue(action, out var distribution))
                {
                    distribution = new StateDistribution();
                    _distributionDictionary.Add(action, distribution);
                }

                return distribution;
            }
        }

        public IEnumerable<StateDistribution> GetAllDistributions() => _distributionDictionary.Values;

        public List<KeyValuePair<string, StateDistribution>> GetAll() => _distributionDictionary.ToList();
    }
}