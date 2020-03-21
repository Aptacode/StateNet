using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Random;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class EnumEngineTests
    {
        private EnumNetwork<DummyStates.States, DummyActions.Actions> GetTestNetwork()
        {
            var network = new EnumNetwork<DummyStates.States, DummyActions.Actions>();

            network.CreateInput(DummyActions.Actions.Play);
            network.CreateInput(DummyActions.Actions.Pause);
            network.CreateInput(DummyActions.Actions.Stop);

            network.SetStart(DummyStates.States.Ready);

            network.Always(DummyStates.States.Ready, DummyActions.Actions.Play, DummyStates.States.Playing);

            network.Clear(DummyStates.States.Ready, DummyActions.Actions.Pause);
            network.Always(DummyStates.States.Ready, DummyActions.Actions.Stop, DummyStates.States.Stopped);

            network.Clear(DummyStates.States.Playing, DummyActions.Actions.Play);
            network.Always(DummyStates.States.Playing, DummyActions.Actions.Pause, DummyStates.States.Paused);
            network.Always(DummyStates.States.Playing, DummyActions.Actions.Stop, DummyStates.States.Stopped);

            network.Always(DummyStates.States.Paused, DummyActions.Actions.Play, DummyStates.States.Playing);
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
            var network = GetTestNetwork();

            var engine =
                new EnumEngine<DummyStates.States, DummyActions.Actions>(new SystemRandomNumberGenerator(), network);

            engine.Start();
            engine.Apply(DummyActions.Actions.Play);
            engine.Apply(DummyActions.Actions.Pause);
            engine.Apply(DummyActions.Actions.Play);
            engine.Apply(DummyActions.Actions.Stop);

            var expectedLog = new List<string> {"Ready", "Playing", "Paused", "Playing", "Stopped"};

            Assert.That(() => engine.GetLog().StateLog.Select(state => state.Name),
                Is.EquivalentTo(expectedLog).After(500).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}