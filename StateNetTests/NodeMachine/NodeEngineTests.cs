using System.Collections.Generic;
using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Attributes;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class NodeEngineTests
    {
        private class DummyGraph : NodeGraph
        {
            [NodeStart("Start")]
            [NodeConnection("Left", "D1")]
            [NodeConnection("Right", "D2")]
            public Node StartTestNode;

            [NodeName("D1")]
            [NodeConnection("Next", "D1", 1)]
            [NodeConnection("Next", "End", 0)]
            public Node Decision1TestNode;

            [NodeName("D2")]
            [NodeConnection("Next", "D1")]
            public Node Decision2TestNode;

            [NodeName("End")]
            public Node EndTestNode;

            public DummyGraph()
            {
                Setup();
            }

            private int decision1Count = 0;

            private void Setup()
            {
                Decision1TestNode.OnUpdateChoosers += (s) =>
                {
                    if (++decision1Count == 2)
                    {
                        s["Next"].Always(EndTestNode);
                    }
                };
            }

        }

        [SetUp]
        public void Setup() { }


        [Test]
        public void EngineTests()
        {
            var graph = new DummyGraph();
            var engine = new NodeEngine(graph);

            Assert.AreEqual(null, engine.CurrentNode);
            engine.Start();
            engine.Apply("Right");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");

            var expectedLog = new List<Node> { graph.StartTestNode, graph.Decision2TestNode, graph.Decision1TestNode, graph.Decision1TestNode, graph.EndTestNode };

            Assert.That(() => expectedLog, Is.EquivalentTo(engine.GetVisitLog()).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }

        private bool canPlay;
        Node ready;
        Node playing;
        Node paused;
        Node stopped;

        private NodeGraph GetPlayerGraph()
        {
            var graph = new NodeGraph();

            ready = graph.GetNode("ready");
            playing = graph.GetNode("playing");
            paused = graph.GetNode("paused");
            stopped = graph.GetNode("stopped");

            graph.StartNode = ready;

            ready.OnUpdateChoosers += (s) =>
            {
                if (canPlay)
                {
                    s["Play"].Always(playing);
                }
                else
                {
                    s["Play"].Invalid();
                }
            };
            ready["Pause"].Invalid();
            ready["Stop"].Always(stopped);

            playing["Play"].Invalid();
            playing["Pause"].Always(paused);
            playing["Stop"].Always(stopped);

            paused.OnUpdateChoosers += (s) =>
            {
                if (canPlay)
                {
                    s["Play"].Always(playing);
                }
                else
                {
                    s["Play"].Invalid();
                }
            };
            paused["Pause"].Invalid();
            paused["Stop"].Always(stopped);

            stopped["Play"].Invalid();
            stopped["Pause"].Invalid();
            stopped["Stop"].Invalid();

            return graph;
        }

        [Test]
        public void EngineTest2s()
        {
            canPlay = true;
            var graph = GetPlayerGraph();

            var engine = new NodeEngine(graph);

            engine.Start();
            engine.Apply("Play");
            engine.Apply("Pause");
            engine.Apply("Play");
            engine.Apply("Stop");

            var expectedLog = new List<Node> { ready, playing, paused, playing, stopped };

            Assert.That(() => expectedLog, Is.EquivalentTo(engine.GetVisitLog()).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}
