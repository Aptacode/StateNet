using System.Collections.Generic;
using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Choosers.Probability;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class Tests
    {
        private void InstantTransition(Node sender) => sender.Exit();

        [Test]
        public void InvalidGraph()
        {
            var nodeGraph = new NodeGraph();
            nodeGraph.SetStart("U1");
            nodeGraph.Add("U1", "U2");
            nodeGraph.Add("U2", "U1");

            Assert.IsFalse(nodeGraph.IsValid());
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test, MaxTime(200)]
        public void TernaryBinaryDistribution()
        {
            var nodeGraph = new NodeGraph();
            nodeGraph.SetStart("T1");
            var T1 = nodeGraph.Add("T1", "U1", "U2", "B1", new TernaryProbabilityChooser(1, 1, 1));
            var U1 = nodeGraph.Add("U1", "T1");
            var U2 = nodeGraph.Add("U2", "T1");
            var B1 = nodeGraph.Add("B1", "T1", "End1", new DeterministicChooser<BinaryChoice>(BinaryChoice.Item1));

            T1.OnVisited += InstantTransition;
            U1.OnVisited += InstantTransition;
            U2.OnVisited += InstantTransition;
            B1.OnVisited += InstantTransition;

            var engine = new NodeEngine(nodeGraph);
            engine.Start();

            engine.OnFinished += (s) =>
            {
                var log = engine.GetVisitLog();
                Assert.Pass();
            };
        }

        [Test, MaxTime(200)]
        public void UnaryTransitionLog()
        {
            var graph = new NodeGraph();
            graph.SetStart("U1");
            var U1 = graph.Add("U1", "U2");
            var U2 = graph.Add("U2", "End");
            var End = graph.GetNode("End");

            var engine = new NodeEngine(graph);

            U1.OnVisited += InstantTransition;
            U2.OnVisited += InstantTransition;

            engine.Start();

            engine.OnFinished += (s) =>
            {
                Assert.That(engine.GetVisitLog(), Is.EquivalentTo(new List<Node> { U1, U2, End }));
            };
        }

        [Test]
        public void ValidEngine()
        {
            var nodeGraph = new NodeGraph();
            nodeGraph.SetStart("U1");
            nodeGraph.Add("U1", "End");

            Assert.IsTrue(nodeGraph.IsValid());
        }
    }
}
