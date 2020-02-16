using System.Collections.Generic;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public enum States { Ready, Playing, Paused, Stopped }
    public enum Actions { Play, Pause, Stop }

    public class EnumEngineTests
    {

        [SetUp]
        public void Setup() { }

        private bool canPlay;
        private Node ready;
        private Node playing;
        private Node paused;
        private Node stopped;

        private EnumNodeGraph<States, Actions> GetPlayerGraph()
        {
            var graph = new EnumNodeGraph<States, Actions>();

            ready = graph[States.Ready];
            playing = graph[States.Playing];
            paused = graph[States.Paused];
            stopped = graph[States.Stopped];

            graph.StartNode = ready;

            ready.OnUpdateChoosers += (s) =>
            {
                if (canPlay)
                {
                    graph[States.Ready, Actions.Play].Always(playing);
                }
                else
                {
                    graph[States.Ready, Actions.Play].Invalid();
                }
            };
            graph[States.Ready, Actions.Pause].Invalid();
            graph[States.Ready, Actions.Stop].Always(stopped);

            graph[States.Playing, Actions.Play].Invalid();
            graph[States.Playing, Actions.Pause].Always(paused);
            graph[States.Playing, Actions.Stop].Always(stopped);

            paused.OnUpdateChoosers += (s) =>
            {
                if (canPlay)
                {
                    graph[States.Paused, Actions.Play].Always(playing);
                }
                else
                {
                    graph[States.Paused, Actions.Play].Invalid();
                }
            };
            graph[States.Paused, Actions.Pause].Invalid();
            graph[States.Paused, Actions.Stop].Always(stopped);

            graph[States.Stopped, Actions.Play].Invalid();
            graph[States.Stopped, Actions.Pause].Invalid();
            graph[States.Stopped, Actions.Stop].Invalid(); return graph;
        }

        [Test]
        public void EngineTest2s()
        {
            canPlay = true;
            var graph = GetPlayerGraph();

            var engine = new EnumNodeEngine<States, Actions>(graph);

            engine.Start();
            engine.Apply(Actions.Play);
            engine.Apply(Actions.Pause);
            engine.Apply(Actions.Play);
            engine.Apply(Actions.Stop);

            var expectedLog = new List<Node> { ready, playing, paused, playing, stopped };

            Assert.That(() => expectedLog, Is.EquivalentTo(engine.GetHistory()).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}
