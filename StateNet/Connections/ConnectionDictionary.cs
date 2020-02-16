using System.Collections.Generic;

namespace Aptacode.StateNet.Connections
{
    public class ConnectionDictionary
    {
        private readonly Dictionary<Node, ActionConnection> _actionConnections = new Dictionary<Node, ActionConnection>();

        public ActionConnection GetActionConnection(Node node)
        {
            if (!_actionConnections.TryGetValue(node, out var nodeConnectionDictionary))
            {
                nodeConnectionDictionary = new ActionConnection();
                _actionConnections.Add(node, nodeConnectionDictionary);
            }

            return nodeConnectionDictionary;
        }

        public ActionConnection this[Node node] => GetActionConnection(node);

        public NodeConnections this[Node node, string action]
        {
            get => GetActionConnection(node)[action];
        }
    }
}
