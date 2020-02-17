using Aptacode.StateNet.Events.Attributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests
{
    public class EngineTests
    {
        private class DummyNetwork : Network
        {
            [StartState("Start")]
            [Connection("Left", "D1")]
            [Connection("Right", "D2")]
            public State StartTestState;

            [StateName("D1")]
            [Connection("Next", "D1", "Static:1")]
            [Connection("Next", "End", "Static:0")]
            public State Decision1TestState;

            [StateName("D2")]
            [Connection("Next", "D1", "VisitCount:D2,2,0,0,2")]
            [Connection("Next", "D2", "VisitCount:D2,2,1,1,0")]
            public State Decision2TestState;

            [StateName("End")]
            public State EndTestState;

            public DummyNetwork() => Setup();

            private int decision1Count = 0;

            private void Setup()
            {
                Decision1TestState.OnUpdateConnections += (s) =>
                                  {
                                      if (++decision1Count == 2)
                                      {
                                          this["D1", "Next"].Always(EndTestState);
                                      }
                                  };
            }
        }

        [Test]
        public void StateHistoryTest()
        {
            var network = new DummyNetwork();
            var engine = new Engine(network);

            Assert.AreEqual(null, engine.CurrentState);
            engine.Start();
            engine.Apply("Right");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");
            engine.Apply("Next");

            var expectedLog = new List<State> { network.StartTestState, network.Decision2TestState, network.Decision2TestState, network.Decision2TestState, network.Decision1TestState, network.Decision1TestState, network.EndTestState };

            Assert.That(() => engine.GetHistory(), Is.EquivalentTo(expectedLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }

        private bool canPlay;
        private State ready;
        private State playing;
        private State paused;
        private State stopped;

        private Network GetTestNetwork()
        {
            var network = new Network();

            ready = network["ready"];
            playing = network["playing"];
            paused = network["paused"];
            stopped = network["stopped"];

            network.StartState = ready;

            ready.OnUpdateConnections += (s) =>
            {
                if (canPlay)
                {
                    network["ready", "Play"].Always(playing);
                }
                else
                {
                    network["ready", "Play"].Invalid();
                }
            };
            network["ready", "Pause"].Invalid();
            network["ready", "Stop"].Always(stopped);

            network["playing", "Play"].Invalid();
            network["playing", "Pause"].Always(paused);
            network["playing", "Stop"].Always(stopped);

            paused.OnUpdateConnections += (s) =>
            {
                if (canPlay)
                {
                    network["paused", "Play"].Always(playing);
                }
                else
                {
                    network["paused", "Play"].Invalid();
                }
            };
            network["paused", "Pause"].Invalid();
            network["paused", "Stop"].Always(stopped);

            network["stopped", "Play"].Invalid();
            network["stopped", "Pause"].Invalid();
            network["stopped", "Stop"].Invalid();

            return network;
        }

        [Test]
        public void EngineTest2s()
        {
            canPlay = true;
            var network = GetTestNetwork();

            var engine = new Engine(network);

            engine.Start();
            engine.Apply("Play");
            engine.Apply("Pause");
            engine.Apply("Play");
            engine.Apply("Stop");

            var expectedLog = new List<State> { ready, playing, paused, playing, stopped };

            Assert.That(() => engine.GetHistory(), Is.EquivalentTo(expectedLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}