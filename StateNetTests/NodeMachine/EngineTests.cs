using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

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
            nodeGraph.DeterministicLink("U1", "U2");
            nodeGraph.DeterministicLink("U2", "U1");

            Assert.IsFalse(nodeGraph.IsValid());
        }

        [SetUp]
        public void Setup() { }

        [Test, MaxTime(200)]
        public void TernaryBinaryDistribution()
        {
            var nodeGraph = new NodeGraph();

            var T1 = nodeGraph.Create("T1");
            var U1 = nodeGraph.Create("U1");
            var U2 = nodeGraph.Create("U2");
            var B1 = nodeGraph.Create("B1");

            nodeGraph.ProbabilisticLink("T1", "U1", "U2", "B1");
            nodeGraph.DeterministicLink("U1", "T1");
            nodeGraph.DeterministicLink("U2", "T1");
            nodeGraph.ProbabilisticLink("B1", "T1", "End");

            nodeGraph.SetStart("T1");
             
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
            var nodeGraph = new NodeGraph();
            nodeGraph.SetStart("U1");
            var U1 = nodeGraph.Create("U1");
            var U2 = nodeGraph.Create("U2");
            var End = nodeGraph.Create("End");

            nodeGraph.DeterministicLink("U1", "U2");
            nodeGraph.DeterministicLink("U2", "End");

            var engine = new NodeEngine(nodeGraph);

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
            nodeGraph.DeterministicLink("U1", "End");

            Assert.IsTrue(nodeGraph.IsValid());
        }
    }
}
