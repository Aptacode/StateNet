using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Random;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class EnumEngineTests
    {
        private EnumStateNetwork<DummyStates.States, DummyInputs.Actions> GetTestNetwork()
        {
            var network = new EnumStateNetwork<DummyStates.States, DummyInputs.Actions>();

            network.CreateInput(DummyInputs.Actions.Play);
            network.CreateInput(DummyInputs.Actions.Pause);
            network.CreateInput(DummyInputs.Actions.Stop);

            network.SetStart(DummyStates.States.Ready);

            network.Always(DummyStates.States.Ready, DummyInputs.Actions.Play, DummyStates.States.Playing);

            network.Clear(DummyStates.States.Ready, DummyInputs.Actions.Pause);
            network.Always(DummyStates.States.Ready, DummyInputs.Actions.Stop, DummyStates.States.Stopped);

            network.Clear(DummyStates.States.Playing, DummyInputs.Actions.Play);
            network.Always(DummyStates.States.Playing, DummyInputs.Actions.Pause, DummyStates.States.Paused);
            network.Always(DummyStates.States.Playing, DummyInputs.Actions.Stop, DummyStates.States.Stopped);

            network.Always(DummyStates.States.Paused, DummyInputs.Actions.Play, DummyStates.States.Playing);
            network.Clear(DummyStates.States.Paused, DummyInputs.Actions.Pause);
            network.Always(DummyStates.States.Paused, DummyInputs.Actions.Stop, DummyStates.States.Stopped);

            network.Clear(DummyStates.States.Stopped, DummyInputs.Actions.Play);
            network.Clear(DummyStates.States.Stopped, DummyInputs.Actions.Pause);
            network.Clear(DummyStates.States.Stopped, DummyInputs.Actions.Stop);
            return network;
        }

        [Test]
        public void EnumEngineHistoryTest()
        {
            var network = GetTestNetwork();

            var engine =
                new EnumStateNetEngine<DummyStates.States, DummyInputs.Actions>(new SystemRandomNumberGenerator(),
                    network);

            engine.Start();
            engine.Apply(DummyInputs.Actions.Play);
            engine.Apply(DummyInputs.Actions.Pause);
            engine.Apply(DummyInputs.Actions.Play);
            engine.Apply(DummyInputs.Actions.Stop);

            var expectedLog = new List<string> {"Ready", "Playing", "Paused", "Playing", "Stopped"};

            Assert.That(() => engine.History.States.Select(state => state.Name),
                Is.EquivalentTo(expectedLog).After(500).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}