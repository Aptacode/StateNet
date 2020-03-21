using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Random;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class EngineTests
    {
        [Test]
        public void StateHistoryTest()
        {
            var network = new DummyNetwork();
            var engine = new Engine(new SystemRandomNumberGenerator(), network);

            Assert.AreEqual(null, engine.CurrentState);
            engine.Start();
            engine.Apply("Right");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");

            var expectedLog = new List<State>
            {
                network.StartTestState, network.Decision2TestState, network.Decision2TestState,
                network.Decision2TestState, network.Decision1TestState, network.EndTestState
            };

            Assert.That(() => engine.GetLog().Log.Select(item => item.Item2),
                Is.EquivalentTo(expectedLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }

        private Network GetTestNetwork()
        {
            var network = new Network();

            network.SetStart("ready");

            network.Always("ready", "Play", "playing");
            network.Always("ready", "Stop", "stopped");
            network.Always("playing", "Pause", "paused");
            network.Always("playing", "Stop", "stopped");
            network.Always("paused", "Play", "playing");
            network.Always("paused", "Stop", "stopped");

            return network;
        }

        [Test]
        public void EngineTest2s()
        {
            var network = GetTestNetwork();

            var engine = new Engine(new SystemRandomNumberGenerator(), network);

            engine.Start();
            engine.Apply("Play");
            engine.Apply("Pause");
            engine.Apply("Play");
            engine.Apply("Stop");

            var expectedLog = new List<(string, string)>
                {("", "ready"), ("Play", "playing"), ("Pause", "paused"), ("Play", "playing"), ("Stop", "stopped")};

            Assert.That(() => engine.GetLog().Log.Select(pair => (pair.Item1.Name, pair.Item2.Name)),
                Is.EquivalentTo(expectedLog).After(200).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}