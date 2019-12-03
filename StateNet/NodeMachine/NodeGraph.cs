using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine
{
    public class NodeGraph
    {
        private readonly Dictionary<string, Node> _nodes;

        public NodeGraph() => _nodes = new Dictionary<string, Node>() ;

        public UnaryNode Add(string sourceName, string destinationName)
        {
            var sourceNode = new UnaryNode(sourceName);
            sourceNode.Visits(GetNode(destinationName));
            _nodes[sourceName] = sourceNode;
            return sourceNode;
        }

        public BinaryNode Add(string sourceName,
                              string destinationName1,
                              string destinationName2,
                              IChooser<BinaryChoice> chooser)
        {
            var sourceNode = new BinaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1), GetNode(destinationName2), chooser);
            return sourceNode;
        }

        public TernaryNode Add(string sourceName,
                               string destinationName1,
                               string destinationName2,
                               string destinationName3,
                               IChooser<TernaryChoice> chooser)
        {
            var sourceNode = new TernaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1), GetNode(destinationName2), GetNode(destinationName3), chooser);
            return sourceNode;
        }

        public QuaternaryNode Add(string sourceName,
                                  string destinationName1,
                                  string destinationName2,
                                  string destinationName3,
                                  string destinationName4,
                                  IChooser<QuaternaryChoice> chooser)
        {
            var sourceNode = new QuaternaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1),
                              GetNode(destinationName2),
                              GetNode(destinationName3),
                              GetNode(destinationName4),
                              chooser);
            return sourceNode;
        }

        public QuinaryNode Add(string sourceName,
                               string destinationName1,
                               string destinationName2,
                               string destinationName3,
                               string destinationName4,
                               string destinationName5,
                               IChooser<QuinaryChoice> chooser)
        {
            var sourceNode = new QuinaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1),
                              GetNode(destinationName2),
                              GetNode(destinationName3),
                              GetNode(destinationName4),
                              GetNode(destinationName5),
                              chooser);
            return sourceNode;
        }

        public HashSet<Node> Flatten(Node node, HashSet<Node> visitedNodes)
        {
            if(!visitedNodes.Contains(node))
            {
                visitedNodes.Add(node);
                foreach(var exitNode in node.GetNextNodes())
                {
                    Flatten(exitNode, visitedNodes);
                }
            }

            return visitedNodes;
        }

        public IEnumerable<Node> GetAll() => _nodes.Select(keyValue => keyValue.Value) ;

        public IEnumerable<EndNode> GetEndNodes() => _nodes.Select(keyValue => keyValue.Value as EndNode)
            .Where(value => value != null) ;

        public Node GetNode(string name)
        {
            if(!_nodes.TryGetValue(name, out var node))
            {
                node = new EndNode(name);
                _nodes.Add(name, node);
            }

            return node;
        }

        public bool IsValid() => GetEndNodes().Any() ;

        public void SetStart(string sourceName) => StartNode = GetNode(sourceName) ;


        public Node StartNode { get; set; }
    }
}
