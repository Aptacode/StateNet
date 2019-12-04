using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine
{
    public class NodeGraph
    {
        private readonly Dictionary<string, Node> _nodes;

        public NodeGraph() => _nodes = new Dictionary<string, Node>();

        public Node Add(string sourceName)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = null;
            return sourceNode;
        }

        public Node Add(string sourceName, List<string> destinationNames)
        {
            switch(destinationNames.Count)
            {
                case 0:

                case 1:
                    throw new Exception();
                case 2:
                    return Add(sourceName, destinationNames[0], destinationNames[1]);
                case 3:
                    return Add(sourceName, destinationNames[0], destinationNames[1], destinationNames[2]);
                case 4:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3]);
                case 5:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4]);
                case 6:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               destinationNames[5]);
                case 7:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               destinationNames[5],
                               destinationNames[6]);
                case 8:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               destinationNames[5],
                               destinationNames[6],
                               destinationNames[7]);
                case 9:
                    return Add(sourceName,
                               destinationNames[0],
                               destinationNames[1],
                               destinationNames[2],
                               destinationNames[3],
                               destinationNames[4],
                               destinationNames[5],
                               destinationNames[6],
                               destinationNames[7],
                               destinationNames[8]);
                default:
                    throw new Exception();
            }
        }

        public Node Add(string sourceName, string destinationName)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new DeterministicChooser<UnaryChoice>(new UnaryOptions(GetNode(destinationName)),
                                                                       UnaryChoice.Item1);
            return sourceNode;
        }

        public Node Add(string sourceName, string destinationName1, string destinationName2)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<BinaryChoice>(new BinaryOptions(GetNode(destinationName1),
                                                                                          GetNode(destinationName2)));
            return sourceNode;
        }

        public Node Add(string sourceName, string destinationName1, string destinationName2, string destinationName3)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<TernaryChoice>(new TernaryOptions(GetNode(destinationName1),
                                                                                            GetNode(destinationName2),
                                                                                            GetNode(destinationName3)));

            return sourceNode;
        }

        public Node Add(string sourceName,
                        string destinationName1,
                        string destinationName2,
                        string destinationName3,
                        string destinationName4)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<QuaternaryChoice>(new QuaternaryOptions(GetNode(destinationName1),
                                                                                                  GetNode(destinationName2),
                                                                                                  GetNode(destinationName3),
                                                                                                  GetNode(destinationName4)));
            return sourceNode;
        }

        public Node Add(string sourceName,
                        string destinationName1,
                        string destinationName2,
                        string destinationName3,
                        string destinationName4,
                        string destinationName5)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<QuinaryChoice>(new QuinaryOptions(GetNode(destinationName1),
                                                                                            GetNode(destinationName2),
                                                                                            GetNode(destinationName3),
                                                                                            GetNode(destinationName4),
                                                                                            GetNode(destinationName5)));

            return sourceNode;
        }

        public Node Add(string sourceName,
                        string destinationName1,
                        string destinationName2,
                        string destinationName3,
                        string destinationName4,
                        string destinationName5,
                        string destinationName6)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<SenaryChoice>(new SenaryOptions(GetNode(destinationName1),
                                                                                          GetNode(destinationName2),
                                                                                          GetNode(destinationName3),
                                                                                          GetNode(destinationName4),
                                                                                          GetNode(destinationName5),
                                                                                          GetNode(destinationName6)));

            return sourceNode;
        }

        public Node Add(string sourceName,
                        string destinationName1,
                        string destinationName2,
                        string destinationName3,
                        string destinationName4,
                        string destinationName5,
                        string destinationName6,
                        string destinationName7)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<SeptenaryChoice>(new SeptenaryOptions(GetNode(destinationName1),
                                                                                                GetNode(destinationName2),
                                                                                                GetNode(destinationName3),
                                                                                                GetNode(destinationName4),
                                                                                                GetNode(destinationName5),
                                                                                                GetNode(destinationName6),
                                                                                                GetNode(destinationName7)));

            return sourceNode;
        }

        public Node Add(string sourceName,
                        string destinationName1,
                        string destinationName2,
                        string destinationName3,
                        string destinationName4,
                        string destinationName5,
                        string destinationName6,
                        string destinationName7,
                        string destinationName8)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<OctaryChoice>(new OctaryOptions(GetNode(destinationName1),
                                                                                          GetNode(destinationName2),
                                                                                          GetNode(destinationName3),
                                                                                          GetNode(destinationName4),
                                                                                          GetNode(destinationName5),
                                                                                          GetNode(destinationName6),
                                                                                          GetNode(destinationName7),
                                                                                          GetNode(destinationName8)));

            return sourceNode;
        }

        public Node Add(string sourceName,
                        string destinationName1,
                        string destinationName2,
                        string destinationName3,
                        string destinationName4,
                        string destinationName5,
                        string destinationName6,
                        string destinationName7,
                        string destinationName8,
                        string destinationName9)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = new ProbabilisticChooser<NonaryChoice>(new NonaryOptions(GetNode(destinationName1),
                                                                                          GetNode(destinationName2),
                                                                                          GetNode(destinationName3),
                                                                                          GetNode(destinationName4),
                                                                                          GetNode(destinationName5),
                                                                                          GetNode(destinationName6),
                                                                                          GetNode(destinationName7),
                                                                                          GetNode(destinationName8),
                                                                                          GetNode(destinationName9)));

            return sourceNode;
        }

        public IEnumerable<Node> GetAll() => _nodes.Select(keyValue => keyValue.Value);

        public IEnumerable<Node> GetEndNodes() => _nodes.Select(keyValue => keyValue.Value)
            .Where(value => value.Chooser == null);

        public Node GetNode(string name)
        {
            if(!_nodes.TryGetValue(name, out var node))
            {
                node = new Node(name);
                _nodes.Add(name, node);
            }

            return node;
        }

        public bool IsValid() => GetEndNodes().Any();

        public void SetNode(Node node) => _nodes[node.Name] = node ;

        public void SetStart(string sourceName) => StartNode = GetNode(sourceName);

        public Node StartNode { get; set; }
    }
}
