using System.Collections.Generic;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public enum States
    {
        Ready,
        Playing,
        Paused,
        Stopped
    }

    public enum Actions
    {
        Play,
        Pause,
        Stop
    }

    public class EnumEngineTests
    {
        private bool canPlay;
        private State paused;
        private State playing;
        private State ready;
        private State stopped;

        private EnumNetwork<States, Actions> GetTestNetwork()
        {
            var network = new EnumNetwork<States, Actions>();

            ready = network[States.Ready];
            playing = network[States.Playing];
            paused = network[States.Paused];
            stopped = network[States.Stopped];

            network.StartState = ready;

            ready.OnUpdateConnections += s =>
            {
                if (canPlay)
                {
                    network[States.Ready, Actions.Play].Always(playing);
                }
                else
                {
                    network[States.Ready, Actions.Play].Invalidate();
                }
            };
            network[States.Ready, Actions.Pause].Invalidate();
            network[States.Ready, Actions.Stop].Always(stopped);

            network[States.Playing, Actions.Play].Invalidate();
            network[States.Playing, Actions.Pause].Always(paused);
            network[States.Playing, Actions.Stop].Always(stopped);

            paused.OnUpdateConnections += s =>
            {
                if (canPlay)
                {
                    network[States.Paused, Actions.Play].Always(playing);
                }
                else
                {
                    network[States.Paused, Actions.Play].Invalidate();
                }
            };
            network[States.Paused, Actions.Pause].Invalidate();
            network[States.Paused, Actions.Stop].Always(stopped);

            network[States.Stopped, Actions.Play].Invalidate();
            network[States.Stopped, Actions.Pause].Invalidate();
            network[States.Stopped, Actions.Stop].Invalidate();
            return network;
        }

        [Test]
        public void EnumEngineHistoryTest()
        {
            canPlay = true;
            var network = GetTestNetwork();

            var engine = new EnumEngine<States, Actions>(network);

            engine.Start();
            engine.Apply(Actions.Play);
            engine.Apply(Actions.Pause);
            engine.Apply(Actions.Play);
            engine.Apply(Actions.Stop);

            var expectedLog = new List<State> {ready, playing, paused, playing, stopped};

            Assert.That(() => engine.GetHistory(),
                Is.EquivalentTo(expectedLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}