using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class Tests
    {
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

        [Test]
        public void TernaryBinaryDistribution()
        {
            var nodeGraph = new NodeGraph();

            nodeGraph.ProbabilisticLink("T1", "U1", "U2", "B1");
            nodeGraph.DeterministicLink("U1", "T1");
            nodeGraph.DeterministicLink("U2", "T1");
            nodeGraph.ProbabilisticLink("B1", "T1", "End");

            nodeGraph.SetStart("T1");

            var engine = new NodeEngine(nodeGraph);

            var hasFinished = false;

            engine.OnFinished += (s) =>
            {
                hasFinished = true;
            };

            engine.Start();

            Assert.That(() => hasFinished, Is.True.After(200, 5));
        }

        [Test]
        public void UnaryTransitionLog()
        {
            var nodeGraph = new NodeGraph();
            var U1 = nodeGraph.Create("U1");
            var U2 = nodeGraph.Create("U2");
            var End = nodeGraph.Create("End");

            nodeGraph.DeterministicLink("U1", "U2");
            nodeGraph.DeterministicLink("U2", "End");
            nodeGraph.SetStart("U1");

            var engine = new NodeEngine(nodeGraph);
            engine.Start();

            Assert.That(() => engine.GetVisitLog(), Is.EquivalentTo(new List<Node> { U1, U2, End }).After(200, 5));
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
