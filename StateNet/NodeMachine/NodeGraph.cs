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

        public NodeGraph() => _nodes = new Dictionary<string, Node>();

        public EndNode Add(string sourceName)
        {
            var sourceNode = new EndNode(sourceName);
            _nodes[sourceName] = sourceNode;
            return sourceNode;
        }

        public UnaryNode Add(string sourceName, string destinationName)
        {
            var sourceNode = new UnaryNode(sourceName);
            sourceNode.Visits(GetNode(destinationName));
            _nodes[sourceName] = sourceNode;
            return sourceNode;
        }

        public Node Add(string sourceName, List<string> destinationNames)
        {
            switch(destinationNames.Count)
            {
                case 0:
                    return Add(sourceName);
                case 1:
                    return Add(sourceName, destinationNames[0]);
                case 2:
                    return Add(sourceName, destinationNames[0], destinationNames[1], null);
                case 3:
                    return Add(sourceName, destinationNames[0], destinationNames[1], destinationNames[2], null);
                case 4:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               null);
                case 5:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               null);
                case 6:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               destinationNames[5],
                               null);
                case 7:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               destinationNames[5],
                               destinationNames[7],
                               null);
                case 8:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               destinationNames[5],
                               destinationNames[7],
                               destinationNames[8],
                               null);
                default:
                    throw new Exception();
            }
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

        public SenaryNode Add(string sourceName,
                              string destinationName1,
                              string destinationName2,
                              string destinationName3,
                              string destinationName4,
                              string destinationName5,
                              string destinationName6,
                              IChooser<SenaryChoice> chooser)
        {
            var sourceNode = new SenaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1),
                              GetNode(destinationName2),
                              GetNode(destinationName3),
                              GetNode(destinationName4),
                              GetNode(destinationName5),
                              GetNode(destinationName6),
                              chooser);
            return sourceNode;
        }

        public SeptenaryNode Add(string sourceName,
                                 string destinationName1,
                                 string destinationName2,
                                 string destinationName3,
                                 string destinationName4,
                                 string destinationName5,
                                 string destinationName6,
                                 string destinationName7,
                                 IChooser<SeptenaryChoice> chooser)
        {
            var sourceNode = new SeptenaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1),
                              GetNode(destinationName2),
                              GetNode(destinationName3),
                              GetNode(destinationName4),
                              GetNode(destinationName5),
                              GetNode(destinationName6),
                              GetNode(destinationName7),
                              chooser);
            return sourceNode;
        }

        public OctaryNode Add(string sourceName,
                              string destinationName1,
                              string destinationName2,
                              string destinationName3,
                              string destinationName4,
                              string destinationName5,
                              string destinationName6,
                              string destinationName7,
                              string destinationName8,
                              IChooser<OctaryChoice> chooser)
        {
            var sourceNode = new OctaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1),
                              GetNode(destinationName2),
                              GetNode(destinationName3),
                              GetNode(destinationName4),
                              GetNode(destinationName5),
                              GetNode(destinationName6),
                              GetNode(destinationName7),
                              GetNode(destinationName8),
                              chooser);
            return sourceNode;
        }

        public NonaryNode Add(string sourceName,
                              string destinationName1,
                              string destinationName2,
                              string destinationName3,
                              string destinationName4,
                              string destinationName5,
                              string destinationName6,
                              string destinationName7,
                              string destinationName8,
                              string destinationName9,
                              IChooser<NonaryChoice> chooser)
        {
            var sourceNode = new NonaryNode(sourceName);
            _nodes[sourceName] = sourceNode;
            sourceNode.Visits(GetNode(destinationName1),
                              GetNode(destinationName2),
                              GetNode(destinationName3),
                              GetNode(destinationName4),
                              GetNode(destinationName5),
                              GetNode(destinationName6),
                              GetNode(destinationName7),
                              GetNode(destinationName8),
                              GetNode(destinationName9),
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

        public IEnumerable<Node> GetAll() => _nodes.Select(keyValue => keyValue.Value);

        public IEnumerable<EndNode> GetEndNodes() => _nodes.Select(keyValue => keyValue.Value as EndNode)
            .Where(value => value != null);

        public Node GetNode(string name)
        {
            if(!_nodes.TryGetValue(name, out var node))
            {
                node = new EndNode(name);
                _nodes.Add(name, node);
            }

            return node;
        }

        public bool IsValid() => GetEndNodes().Any();

        public void SetStart(string sourceName) => StartNode = GetNode(sourceName);


        public Node StartNode { get; set; }
    }
}
