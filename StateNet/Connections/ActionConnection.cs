using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Connections
{
    public class ActionConnection
    {
        private readonly Dictionary<string, NodeConnections> _nodeCollectionDictionary = new Dictionary<string, NodeConnections>();

        public NodeConnections this[string action]
        {
            get
            {
                if (!_nodeCollectionDictionary.TryGetValue(action, out var nodeConnection))
                {
                    nodeConnection = new NodeConnections();
                    _nodeCollectionDictionary.Add(action, nodeConnection);
                }

                return nodeConnection;
            }
        }

        public IEnumerable<NodeConnections> GetAllNodeConnections() => _nodeCollectionDictionary.Values;

        public List<KeyValuePair<string, NodeConnections>> GetAll() => _nodeCollectionDictionary.ToList();
    }
}