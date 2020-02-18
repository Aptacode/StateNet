using System.Collections.Generic;
using Aptacode.StateNet.Attributes;
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

            var expectedLog = new List<State>
            {
                network.StartTestState, network.Decision2TestState, network.Decision2TestState,
                network.Decision2TestState, network.Decision1TestState, network.Decision1TestState, network.EndTestState
            };

            Assert.That(() => engine.GetHistory(),
                Is.EquivalentTo(expectedLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }

        private Network GetTestNetwork()
        {
            var network = new Network();

            ready = network["ready"];
            playing = network["playing"];
            paused = network["paused"];
            stopped = network["stopped"];

            network.StartState = ready;

            ready.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network["ready", "Play"].Always(playing);
                }
                else
                {
                    network["ready", "Play"].Invalidate();
                }
            };
            network["ready", "Pause"].Invalidate();
            network["ready", "Stop"].Always(stopped);

            network["playing", "Play"].Invalidate();
            network["playing", "Pause"].Always(paused);
            network["playing", "Stop"].Always(stopped);

            paused.OnUpdateConnections += delegate
            {
                if (canPlay)
                {
                    network["paused", "Play"].Always(playing);
                }
                else
                {
                    network["paused", "Play"].Invalidate();
                }
            };
            network["paused", "Pause"].Invalidate();
            network["paused", "Stop"].Always(stopped);

            network["stopped", "Play"].Invalidate();
            network["stopped", "Pause"].Invalidate();
            network["stopped", "Stop"].Invalidate();

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

            var expectedLog = new List<State> {ready, playing, paused, playing, stopped};

            Assert.That(() => engine.GetHistory(),
                Is.EquivalentTo(expectedLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }

        private class DummyNetwork : Network
        {
            private int decision1Count;

            [StateName("D1")] [Connection("Next", "D1", "Static:1")] [Connection("Next", "End", "Static:0")]
            public State Decision1TestState;

            [StateName("D2")]
            [Connection("Next", "D1", "VisitCount:D2,2,0,0,2")]
            [Connection("Next", "D2", "VisitCount:D2,2,1,1,0")]
            public State Decision2TestState;

            [StateName("End")] public State EndTestState;

            [StartState("Start")] [Connection("Left", "D1")] [Connection("Right", "D2")]
            public State StartTestState;

            public DummyNetwork()
            {
                Setup();
            }

            private void Setup()
            {
                Decision1TestState.OnUpdateConnections += delegate
                {
                    if (++decision1Count == 2)
                    {
                        this["D1", "Next"].Always(EndTestState);
                    }
                };
            }
        }
    }
}