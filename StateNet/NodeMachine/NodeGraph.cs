using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Choosers.Deterministic;
using Aptacode.StateNet.NodeMachine.Choosers.Probabilistic;
using Aptacode.StateNet.NodeMachine.Events;
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

        private void InstantTransition(Node sender) => sender.Exit() ;

        public Node Create(string sourceName)
        {
            var sourceNode = GetNode(sourceName);
            sourceNode.Chooser = null;
            return sourceNode;
        }

        public NodeChooser DeterministicLink(string from)
        {
            var fromNode = GetNode(from);
            fromNode.Chooser = null;
            return null;
        }

        public NodeChooser DeterministicLink(string from, List<string> options)
        {
            switch(options.Count)
            {
                case 0:
                    return DeterministicLink(from);
                case 1:
                    return DeterministicLink(from, options[0]);
                case 2:
                    return DeterministicLink(from, options[0], options[1]);
                case 3:
                    return DeterministicLink(from, options[0], options[1], options[2]);
                case 4:
                    return DeterministicLink(from, options[0], options[1], options[2], options[3]);
                case 5:
                    return DeterministicLink(from, options[0], options[1], options[2], options[3], options[4]);
                case 6:
                    return DeterministicLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5]);
                case 7:
                    return DeterministicLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5],
                                             options[6]);
                case 8:
                    return DeterministicLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5],
                                             options[6],
                                             options[7]);
                case 9:
                    return DeterministicLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5],
                                             options[6],
                                             options[7],
                                             options[8]);

                default:
                    throw new Exception();
            }
        }

        public UnaryDeterministicChooser DeterministicLink(string from, string option1)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);

            var chooser = new UnaryDeterministicChooser(option1Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public BinaryDeterministicChooser DeterministicLink(string from, string option1, string option2)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);

            var chooser = new BinaryDeterministicChooser(option1Node, option2Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public TernaryDeterministicChooser DeterministicLink(string from,
                                                             string option1,
                                                             string option2,
                                                             string option3)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);

            var chooser = new TernaryDeterministicChooser(option1Node, option2Node, option3Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public QuaternaryDeterministicChooser DeterministicLink(string from,
                                                                string option1,
                                                                string option2,
                                                                string option3,
                                                                string option4)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);

            var chooser = new QuaternaryDeterministicChooser(option1Node, option2Node, option3Node, option4Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public QuinaryDeterministicChooser DeterministicLink(string from,
                                                             string option1,
                                                             string option2,
                                                             string option3,
                                                             string option4,
                                                             string option5)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);

            var chooser = new QuinaryDeterministicChooser(option1Node,
                                                          option2Node,
                                                          option3Node,
                                                          option4Node,
                                                          option5Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public SenaryDeterministicChooser DeterministicLink(string from,
                                                            string option1,
                                                            string option2,
                                                            string option3,
                                                            string option4,
                                                            string option5,
                                                            string option6)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);

            var chooser = new SenaryDeterministicChooser(option1Node,
                                                         option2Node,
                                                         option3Node,
                                                         option4Node,
                                                         option5Node,
                                                         option6Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public SeptenaryDeterministicChooser DeterministicLink(string from,
                                                               string option1,
                                                               string option2,
                                                               string option3,
                                                               string option4,
                                                               string option5,
                                                               string option6,
                                                               string option7)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);
            var option7Node = GetNode(option7);

            var chooser = new SeptenaryDeterministicChooser(option1Node,
                                                            option2Node,
                                                            option3Node,
                                                            option4Node,
                                                            option5Node,
                                                            option6Node,
                                                            option7Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public OctaryDeterministicChooser DeterministicLink(string from,
                                                            string option1,
                                                            string option2,
                                                            string option3,
                                                            string option4,
                                                            string option5,
                                                            string option6,
                                                            string option7,
                                                            string option8)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);
            var option7Node = GetNode(option7);
            var option8Node = GetNode(option8);

            var chooser = new OctaryDeterministicChooser(option1Node,
                                                         option2Node,
                                                         option3Node,
                                                         option4Node,
                                                         option5Node,
                                                         option6Node,
                                                         option7Node,
                                                         option8Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public NonaryDeterministicChooser DeterministicLink(string from,
                                                            string option1,
                                                            string option2,
                                                            string option3,
                                                            string option4,
                                                            string option5,
                                                            string option6,
                                                            string option7,
                                                            string option8,
                                                            string option9)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);
            var option7Node = GetNode(option7);
            var option8Node = GetNode(option8);
            var option9Node = GetNode(option9);

            var chooser = new NonaryDeterministicChooser(option1Node,
                                                         option2Node,
                                                         option3Node,
                                                         option4Node,
                                                         option5Node,
                                                         option6Node,
                                                         option7Node,
                                                         option8Node,
                                                         option9Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public IEnumerable<Node> GetAll() => _nodes.Select(keyValue => keyValue.Value);

        public IEnumerable<Node> GetEndNodes() => _nodes.Select(keyValue => keyValue.Value)
            .Where(value => value.Chooser == null);

        public Node GetNode(string name)
        {
            if(!_nodes.TryGetValue(name, out var node))
            {
                node = new Node(name);
                node.OnVisited += InstantTransition;
                _nodes.Add(name, node);
            }

            return node;
        }

        public bool IsValid() => GetEndNodes().Any();

        public NodeChooser ProbabilisticLink(string from, List<string> options)
        {
            switch(options.Count)
            {
                case 2:
                    return ProbabilisticLink(from, options[0], options[1]);
                case 3:
                    return ProbabilisticLink(from, options[0], options[1], options[2]);
                case 4:
                    return ProbabilisticLink(from, options[0], options[1], options[2], options[3]);
                case 5:
                    return ProbabilisticLink(from, options[0], options[1], options[2], options[3], options[4]);
                case 6:
                    return ProbabilisticLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5]);
                case 7:
                    return ProbabilisticLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5],
                                             options[6]);
                case 8:
                    return ProbabilisticLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5],
                                             options[6],
                                             options[7]);
                case 9:
                    return ProbabilisticLink(from,
                                             options[0],
                                             options[1],
                                             options[2],
                                             options[3],
                                             options[4],
                                             options[5],
                                             options[6],
                                             options[7],
                                             options[8]);

                default:
                    throw new Exception();
            }
        }

        public BinaryProbabilisticChooser ProbabilisticLink(string from, string option1, string option2)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);

            var chooser = new BinaryProbabilisticChooser(option1Node, option2Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public TernaryProbabilisticChooser ProbabilisticLink(string from,
                                                             string option1,
                                                             string option2,
                                                             string option3)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);

            var chooser = new TernaryProbabilisticChooser(option1Node, option2Node, option3Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public QuaternaryProbabilisticChooser ProbabilisticLink(string from,
                                                                string option1,
                                                                string option2,
                                                                string option3,
                                                                string option4)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);

            var chooser = new QuaternaryProbabilisticChooser(option1Node, option2Node, option3Node, option4Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public QuinaryProbabilisticChooser ProbabilisticLink(string from,
                                                             string option1,
                                                             string option2,
                                                             string option3,
                                                             string option4,
                                                             string option5)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);

            var chooser = new QuinaryProbabilisticChooser(option1Node,
                                                          option2Node,
                                                          option3Node,
                                                          option4Node,
                                                          option5Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public SenaryProbabilisticChooser ProbabilisticLink(string from,
                                                            string option1,
                                                            string option2,
                                                            string option3,
                                                            string option4,
                                                            string option5,
                                                            string option6)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);

            var chooser = new SenaryProbabilisticChooser(option1Node,
                                                         option2Node,
                                                         option3Node,
                                                         option4Node,
                                                         option5Node,
                                                         option6Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public SeptenaryProbabilisticChooser ProbabilisticLink(string from,
                                                               string option1,
                                                               string option2,
                                                               string option3,
                                                               string option4,
                                                               string option5,
                                                               string option6,
                                                               string option7)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);
            var option7Node = GetNode(option7);

            var chooser = new SeptenaryProbabilisticChooser(option1Node,
                                                            option2Node,
                                                            option3Node,
                                                            option4Node,
                                                            option5Node,
                                                            option6Node,
                                                            option7Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public OctaryProbabilisticChooser ProbabilisticLink(string from,
                                                            string option1,
                                                            string option2,
                                                            string option3,
                                                            string option4,
                                                            string option5,
                                                            string option6,
                                                            string option7,
                                                            string option8)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);
            var option7Node = GetNode(option7);
            var option8Node = GetNode(option8);

            var chooser = new OctaryProbabilisticChooser(option1Node,
                                                         option2Node,
                                                         option3Node,
                                                         option4Node,
                                                         option5Node,
                                                         option6Node,
                                                         option7Node,
                                                         option8Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public NonaryProbabilisticChooser ProbabilisticLink(string from,
                                                            string option1,
                                                            string option2,
                                                            string option3,
                                                            string option4,
                                                            string option5,
                                                            string option6,
                                                            string option7,
                                                            string option8,
                                                            string option9)
        {
            var fromNode = GetNode(from);
            var option1Node = GetNode(option1);
            var option2Node = GetNode(option2);
            var option3Node = GetNode(option3);
            var option4Node = GetNode(option4);
            var option5Node = GetNode(option5);
            var option6Node = GetNode(option6);
            var option7Node = GetNode(option7);
            var option8Node = GetNode(option8);
            var option9Node = GetNode(option9);

            var chooser = new NonaryProbabilisticChooser(option1Node,
                                                         option2Node,
                                                         option3Node,
                                                         option4Node,
                                                         option5Node,
                                                         option6Node,
                                                         option7Node,
                                                         option8Node,
                                                         option9Node);
            fromNode.Chooser = chooser;
            return chooser;
        }

        public void SetStart(string sourceName) => StartNode = GetNode(sourceName);

        public void SubscribeOnVisited(string nodeName, NodeEvent OnVisits)
        {
            var node = GetNode(nodeName);
            node.OnVisited -= InstantTransition;
            node.OnVisited += OnVisits;
        }

        public void UnsubscribeOnVisited(string nodeName, NodeEvent OnVisits)
        {
            var node = GetNode(nodeName);
            node.OnVisited -= OnVisits;
        }

        public Node StartNode { get; set; }
    }
}
