using Aptacode.StateNet.NodeMachine.Attributes;
using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Events;
using Aptacode.StateNet.NodeMachine.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Aptacode.StateNet.NodeMachine
{
    public class NodeGraph
    {
        private readonly Dictionary<string, Node> _nodes;

        public NodeGraph()
        {
            _nodes = new Dictionary<string, Node>();

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
                this.GetChooser(node, connectionInfo.ActionName).SetWeight(this.GetNode(connectionInfo.TargetName), connectionInfo.ConnectionChance);
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

        public IEnumerable<Node> GetAll() => _nodes.Select(keyValue => keyValue.Value);

        public IEnumerable<Node> GetEndNodes() => _nodes.Select(keyValue => keyValue.Value)
            .Where(value => value.IsEndNode);

        public Node GetNode(string name)
        {
            if(!_nodes.TryGetValue(name, out var node))
            {
                node = new Node(name);
                _nodes.Add(name, node);
            }

            return node;
        }

        public NodeChooser GetChooser(Node node, string actionName)
        {
            var chooser = node[actionName];
            if(chooser == null)
            {
                chooser = new NodeChooser();
                node[actionName] = chooser;
            }

            return chooser;
        }

        public bool IsValid() => GetEndNodes().Any();

        public Node StartNode { get; set; }
    }
}
