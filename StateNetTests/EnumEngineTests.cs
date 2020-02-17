using NUnit.Framework;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests
{
    public enum States { Ready, Playing, Paused, Stopped }

    public enum Actions { Play, Pause, Stop }

    public class EnumEngineTests
    {
        private bool canPlay;
        private State ready;
        private State playing;
        private State paused;
        private State stopped;

        private EnumNetwork<States, Actions> GetTestNetwork()
        {
            var network = new EnumNetwork<States, Actions>();

            ready = network[States.Ready];
            playing = network[States.Playing];
            paused = network[States.Paused];
            stopped = network[States.Stopped];

            network.StartState = ready;

            ready.OnUpdateConnections += (s) =>
            {
                if (canPlay)
                {
                    network[States.Ready, Actions.Play].Always(playing);
                }
                else
                {
                    network[States.Ready, Actions.Play].Invalid();
                }
            };
            network[States.Ready, Actions.Pause].Invalid();
            network[States.Ready, Actions.Stop].Always(stopped);

            network[States.Playing, Actions.Play].Invalid();
            network[States.Playing, Actions.Pause].Always(paused);
            network[States.Playing, Actions.Stop].Always(stopped);

            paused.OnUpdateConnections += (s) =>
            {
                if (canPlay)
                {
                    network[States.Paused, Actions.Play].Always(playing);
                }
                else
                {
                    network[States.Paused, Actions.Play].Invalid();
                }
            };
            network[States.Paused, Actions.Pause].Invalid();
            network[States.Paused, Actions.Stop].Always(stopped);

            network[States.Stopped, Actions.Play].Invalid();
            network[States.Stopped, Actions.Pause].Invalid();
            network[States.Stopped, Actions.Stop].Invalid(); return network;
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

            var expectedLog = new List<State> { ready, playing, paused, playing, stopped };

            Assert.That(() => engine.GetHistory(), Is.EquivalentTo(expectedLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}