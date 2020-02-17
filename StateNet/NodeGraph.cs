using Aptacode.StateNet.Connections;
using Aptacode.StateNet.Events.Attributes;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.NodeWeights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Aptacode.StateNet
{
    public class NodeGraph : INodeGraph
    {
        protected readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();
        protected readonly ConnectionDictionary _actionConnections = new ConnectionDictionary();

        public IEnumerable<Node> GetAll() => _nodes.Select(keyValue => keyValue.Value);

        public IEnumerable<Node> GetEndNodes() => _nodes.Select(keyValue => keyValue.Value).Where(IsEndNode);

        public bool IsValid() => GetEndNodes().Any();

        public Node StartNode { get; set; }

        public Node GetNode(string name)
        {
            if (!_nodes.TryGetValue(name, out var node))
            {
                node = new Node(name);
                _nodes.Add(name, node);
            }

            return node;
        }

        public Node this[string nodeName] => GetNode(nodeName);

        public NodeConnections this[string nodeName, string action]
        {
            get => _actionConnections[GetNode(nodeName), action];
        }

        public NodeConnections this[Node node, string action]
        {
            get => _actionConnections[node, action];
        }

        public bool IsEndNode(Node node) => _actionConnections[node].GetAllNodeConnections().All((chooser) => chooser.IsInvalid);

        public NodeGraph()
        {
            ActOnFieldAttributes(typeof(NodeNameAttribute), (field, attribute) =>
            {
                field.SetValue(this, GetNode(((NodeNameAttribute)attribute).Name));
            });

            ActOnFieldAttributes(typeof(NodeStartAttribute), (field, attribute) =>
            {
                var node = GetNode(((NodeStartAttribute)attribute).Name);
                field.SetValue(this, node);
                StartNode = node;
            });

            ActOnFieldAttributes(typeof(NodeConnectionAttribute), (field, attribute) =>
            {
                var connectionInfo = (NodeConnectionAttribute)attribute;
                var node = (Node)field.GetValue(this);

                this[node.Name, connectionInfo.ActionName].UpdateWeight(GetNode(connectionInfo.TargetName), ConnectionWeightFactory.FromString(connectionInfo.ConnectionDescription));
            });
        }

        private void ActOnFieldAttributes(Type targetType, Action<FieldInfo, object> doWhenFound)
        {
            var typeInfo = GetType().GetTypeInfo();

            foreach (var field in typeInfo.GetRuntimeFields())
            {
                foreach (var attr in field.GetCustomAttributes(true))
                {
                    if (attr.GetType() == targetType)
                    {
                        doWhenFound(field, attr);
                    }
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var node in _nodes.Values.ToList())
            {
                stringBuilder.AppendLine(node.Name);

                var actionChoosers = _actionConnections.GetActionConnection(node).GetAll();
                if (actionChoosers.Any())
                {
                    stringBuilder.AppendLine($"({actionChoosers[0].Key}->{actionChoosers[0].Value})");
                    for (var i = 1; i < actionChoosers.Count; i++)
                    {
                        stringBuilder.AppendLine($",({actionChoosers[i].Key}->{actionChoosers[i].Value})");
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}