using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Random;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class EngineTests
    {
        private bool canPlay;
        private State paused;
        private State playing;
        private State ready;
        private State stopped;

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

            ready = network["ready"];
            playing = network["playing"];
            paused = network["paused"];
            stopped = network["stopped"];

            network.SetStart(ready);

            ready.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network.Always("ready", "Play", playing.Name);
                }
                else
                {
                    network.Clear("ready", "Play");
                }
            };
            network.Clear("ready", "Pause");
            network.Always("ready", "Stop", stopped.Name);

            network.Always("playing", "Pause", paused.Name);
            network.Always("playing", "Stop", stopped.Name);

            paused.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network.Always("paused", "Play", playing.Name);
                }
                else
                {
                    network.Clear("paused", "Play");
                }
            };
            network.Always("paused", "Stop", stopped.Name);

            return network;
        }

        [Test]
        public void EngineTest2s()
        {
            canPlay = true;
            var network = GetTestNetwork();

            var engine = new Engine(new SystemRandomNumberGenerator(), network);

            var play = network.GetInput("Play");
            var pause = network.GetInput("Pause");
            var stop = network.GetInput("Stop");

            engine.Start();
            engine.Apply("Play");
            engine.Apply("Pause");
            engine.Apply("Play");
            engine.Apply("Stop");

            var expectedLog = new List<(Input, State)>
                {(Input.Empty, ready), (play, playing), (pause, paused), (play, playing), (stop, stopped)};

            Assert.That(() => engine.GetLog().Log,
                Is.EquivalentTo(expectedLog).After(200).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}