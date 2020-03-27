using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine
{
    public class ConnectionDistribution
    {
        private readonly List<(Connection, int)> _connectionWeights = new List<(Connection, int)>();

        public void Add(Connection connection, int weight)
        {
            _connectionWeights.Add((connection, weight));
        }

        public int SumWeights()
        {
            return _connectionWeights.Sum(pair => pair.Item2 >= 0 ? pair.Item2 : 0);
        }

        public IEnumerator<(Connection, int)> GetEnumerator()
        {
            return _connectionWeights.GetEnumerator();
        }
    }
}