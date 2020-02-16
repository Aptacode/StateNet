using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Aptacode.StateNet.Events.Attributes;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{

    public class NodeChooserCollection
    {
        private readonly Dictionary<Node, Dictionary<string, NodeChooser>> _nodeChoosers = new Dictionary<Node, Dictionary<string, NodeChooser>>();

        public Dictionary<string, NodeChooser> GetNodeChoosers(Node node)
        {
            if (!_nodeChoosers.TryGetValue(node, out var chooserDictionary))
            {
                chooserDictionary = new Dictionary<string, NodeChooser>();
                _nodeChoosers.Add(node, chooserDictionary);
            }

            return chooserDictionary;
        }

        public Dictionary<string, NodeChooser> this[Node node] => GetNodeChoosers(node);

        public NodeChooser this[Node node, string action]
        {
            get
            {
                var nodeChooserDictionary = GetNodeChoosers(node);
                if (!nodeChooserDictionary.TryGetValue(action, out var nodeChooser))
                {
                    nodeChooser = new NodeChooser();
                    nodeChooserDictionary.Add(action, nodeChooser);
                }

                return nodeChooser;
            }

            set => GetNodeChoosers(node)[action] = value;
        }

    }

    public class NodeGraph : INodeGraph
    {
        protected readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();
        protected readonly NodeChooserCollection _nodeChooserCollection = new NodeChooserCollection();

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
        public NodeChooser this[string nodeName, string action]
        {
            get => _nodeChooserCollection[GetNode(nodeName), action];
            set => _nodeChooserCollection[GetNode(nodeName), action] = value;
        }
        public NodeChooser this[Node node, string action]
        {
            get => _nodeChooserCollection[node, action];
            set => _nodeChooserCollection[node, action] = value;
        }

        public bool IsEndNode(Node node) => _nodeChooserCollection.GetNodeChoosers(node).Values.Count((chooser) => !chooser.IsInvalid) == 0;

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
                this[node.Name, connectionInfo.ActionName].UpdateWeight(GetNode(connectionInfo.TargetName), connectionInfo.ConnectionChance);
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

                var actionChoosers = _nodeChooserCollection.GetNodeChoosers(node).ToList();
                if (actionChoosers.Count > 0)
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
