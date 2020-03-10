using System.Collections.Generic;
using Aptacode.StateNet.Random;
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

            ready.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network[States.Ready, Actions.Play].Always(playing);
                }
                else
                {
                    network[States.Ready, Actions.Play].Clear();
                }
            };
            network[States.Ready, Actions.Pause].Clear();
            network[States.Ready, Actions.Stop].Always(stopped);

            network[States.Playing, Actions.Play].Clear();
            network[States.Playing, Actions.Pause].Always(paused);
            network[States.Playing, Actions.Stop].Always(stopped);

            paused.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network[States.Paused, Actions.Play].Always(playing);
                }
                else
                {
                    network[States.Paused, Actions.Play].Clear();
                }
            };
            network[States.Paused, Actions.Pause].Clear();
            network[States.Paused, Actions.Stop].Always(stopped);

            network[States.Stopped, Actions.Play].Clear();
            network[States.Stopped, Actions.Pause].Clear();
            network[States.Stopped, Actions.Stop].Clear();
            return network;
        }

        [Test]
        public void EnumEngineHistoryTest()
        {
            canPlay = true;
            var network = GetTestNetwork();

            var engine = new EnumEngine<States, Actions>(new SystemRandomNumberGenerator(), network);

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