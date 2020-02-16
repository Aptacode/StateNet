using System.Collections.Generic;
using Aptacode.StateNet.Events.Attributes;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
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

            public DummyGraph() => Setup();

            private int decision1Count = 0;

            private void Setup() => Decision1TestNode.OnUpdateChoosers += (s) =>
                                  {
                                      if (++decision1Count == 2)
                                      {
                                          this["D1", "Next"].Always(EndTestNode);
                                      }
                                  };

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

            Assert.That(() => expectedLog, Is.EquivalentTo(engine.GetHistory()).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }

        private bool canPlay;
        private Node ready;
        private Node playing;
        private Node paused;
        private Node stopped;

        private NodeGraph GetPlayerGraph()
        {
            var graph = new NodeGraph();

            ready = graph["ready"];
            playing = graph["playing"];
            paused = graph["paused"];
            stopped = graph["stopped"];

            graph.StartNode = ready;

            ready.OnUpdateChoosers += (s) =>
            {
                if (canPlay)
                {
                    graph["ready", "Play"].Always(playing);
                }
                else
                {
                    graph["ready", "Play"].Invalid();
                }
            };
            graph["ready", "Pause"].Invalid();
            graph["ready", "Stop"].Always(stopped);

            graph["playing", "Play"].Invalid();
            graph["playing", "Pause"].Always(paused);
            graph["playing", "Stop"].Always(stopped);

            paused.OnUpdateChoosers += (s) =>
            {
                if (canPlay)
                {
                    graph["paused", "Play"].Always(playing);
                }
                else
                {
                    graph["paused", "Play"].Invalid();
                }
            };
            graph["paused", "Pause"].Invalid();
            graph["paused", "Stop"].Always(stopped);

            graph["stopped", "Play"].Invalid();
            graph["stopped", "Pause"].Invalid();
            graph["stopped", "Stop"].Invalid();

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

            Assert.That(() => expectedLog, Is.EquivalentTo(engine.GetHistory()).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}
