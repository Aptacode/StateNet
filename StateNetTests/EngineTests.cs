﻿using System.Collections.Generic;
using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.Random;
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
                    network["ready", "Play"].Clear();
                }
            };
            network["ready", "Pause"].Clear();
            network["ready", "Stop"].Always(stopped);

            network["playing", "Play"].Clear();
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
                    network["paused", "Play"].Clear();
                }
            };
            network["paused", "Pause"].Clear();
            network["paused", "Stop"].Always(stopped);

            network["stopped", "Play"].Clear();
            network["stopped", "Pause"].Clear();
            network["stopped", "Stop"].Clear();

            return network;
        }

        [Test]
        public void EngineTest2s()
        {
            canPlay = true;
            var network = GetTestNetwork();

            var engine = new Engine(new SystemRandomNumberGenerator(), network);

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

            public DummyNetwork()
            {
                Setup();
            }

            [StateName("D1")]
            [Connection("Next", "D1", "StaticWeight:1")]
            [Connection("Next", "End", "StaticWeight:0")]
            public State Decision1TestState { get; set; }

            [StateName("D2")]
            [Connection("Next", "D1", "VisitCountWeight:D2,2,0,0,2")]
            [Connection("Next", "D2", "VisitCountWeight:D2,2,1,1,0")]
            public State Decision2TestState { get; set; }

            [StateName("End")] public State EndTestState { get; set; }

            [StartState("Start")]
            [Connection("Left", "D1")]
            [Connection("Right", "D2")]
            public State StartTestState { get; set; }

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