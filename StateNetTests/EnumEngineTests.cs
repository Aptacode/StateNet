using System.Collections.Generic;
using Aptacode.StateNet.Random;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class EnumEngineTests
    {
        private bool canPlay;
        private State paused;
        private State playing;
        private State ready;
        private State stopped;

        private EnumNetwork<DummyStates.States, DummyActions.Actions> GetTestNetwork()
        {
            var network = new EnumNetwork<DummyStates.States, DummyActions.Actions>();

            ready = network[DummyStates.States.Ready];
            playing = network[DummyStates.States.Playing];
            paused = network[DummyStates.States.Paused];
            stopped = network[DummyStates.States.Stopped];

            network.CreateInput(DummyActions.Actions.Play.ToString());
            network.CreateInput(DummyActions.Actions.Pause.ToString());
            network.CreateInput(DummyActions.Actions.Stop.ToString());

            network.SetStart(ready);

            ready.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network.Always(DummyStates.States.Ready, DummyActions.Actions.Play, DummyStates.States.Playing);
                }
                else
                {
                    network.Clear(DummyStates.States.Ready, DummyActions.Actions.Play);
                }
            };
            network.Clear(DummyStates.States.Ready, DummyActions.Actions.Pause);
            network.Always(DummyStates.States.Ready, DummyActions.Actions.Stop, DummyStates.States.Stopped);

            network.Clear(DummyStates.States.Playing, DummyActions.Actions.Play);
            network.Always(DummyStates.States.Playing, DummyActions.Actions.Pause, DummyStates.States.Paused);
            network.Always(DummyStates.States.Playing, DummyActions.Actions.Stop, DummyStates.States.Stopped);

            paused.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network.Always(DummyStates.States.Paused, DummyActions.Actions.Play, DummyStates.States.Playing);
                }
                else
                {
                    network.Clear(DummyStates.States.Paused, DummyActions.Actions.Play);
                }
            };
            network.Clear(DummyStates.States.Paused, DummyActions.Actions.Pause);
            network.Always(DummyStates.States.Paused, DummyActions.Actions.Stop, DummyStates.States.Stopped);

            network.Clear(DummyStates.States.Stopped, DummyActions.Actions.Play);
            network.Clear(DummyStates.States.Stopped, DummyActions.Actions.Pause);
            network.Clear(DummyStates.States.Stopped, DummyActions.Actions.Stop);
            return network;
        }

        [Test]
        public void EnumEngineHistoryTest()
        {
            canPlay = true;
            var network = GetTestNetwork();

            var engine =
                new EnumEngine<DummyStates.States, DummyActions.Actions>(new SystemRandomNumberGenerator(), network);

            engine.Start();
            engine.Apply(DummyActions.Actions.Play);
            engine.Apply(DummyActions.Actions.Pause);
            engine.Apply(DummyActions.Actions.Play);
            engine.Apply(DummyActions.Actions.Stop);

            var expectedLog = new List<State> {ready, playing, paused, playing, stopped};

            Assert.That(() => engine.GetLog().StateLog,
                Is.EquivalentTo(expectedLog).After(500).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}