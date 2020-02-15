using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aptacode.StateNet.Events.Attributes;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{
    public class NodeGraph : INodeGraph
    {
        protected readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();
        protected readonly Dictionary<Node, Dictionary<string, NodeChooser>> _Choosers = new Dictionary<Node, Dictionary<string, NodeChooser>>();

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
        public Dictionary<string, NodeChooser> GetChoosers(Node node)
        {
            if (!_Choosers.TryGetValue(node, out var choosers))
            {
                choosers = new Dictionary<string, NodeChooser>();
                _Choosers.Add(node, choosers);
            }

            return choosers;
        }

        public Node this[string state] => GetNode(state);
        public NodeChooser this[string state, string action]
        {
            get
            {
                var choosers = GetChoosers(GetNode(state));
                if (choosers.TryGetValue(action, out var value))
                {
                    return value;
                }
                else
                {
                    var nodeChooser = new NodeChooser();
                    choosers.Add(action, nodeChooser);
                    return nodeChooser;
                }
            }

            set => GetChoosers(GetNode(state))[action] = value;
        }

        public Node Next(Node node, string actionName)
        {
            if (GetChoosers(node).TryGetValue(actionName, out var chooser))
            {
                return chooser?.Next();
            }
            else
            {
                return null;
            }
        }
        public bool IsEndNode(Node node) => GetChoosers(node).Count(c => c.Value.TotalWeight > 0) == 0;


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


    }
}
