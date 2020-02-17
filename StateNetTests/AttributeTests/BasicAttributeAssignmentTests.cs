using Aptacode.StateNet.Events.Attributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests
{
    /// <summary>
    /// Checks if the various states in a network are being assigned to correctly.
    /// Primary focus is testing that the *initial* Network reflection is behaving as expected.
    /// </summary>
    public class BasicAttributeAssignmentTests
    {
        private class DummyNetwork : Network
        {
            [StartState("Start")]
            [Connection("Next", "End")]
            public State StartState;

            [StateName("End")]
            public State EndState;
        }

        [Test]
        public void StatesCreated()
        {
            var network = new DummyNetwork();
            var states = new List<State>(network.GetAll());

            Assert.AreEqual(2, states.Count);
            Assert.AreEqual("Start", network.StartState.Name);
            Assert.AreEqual("End", network.EndState.Name);
        }

        [Test]
        public void IsStartStateSet()
        {
            var nodeGraph = new DummyNetwork();

            Assert.AreEqual("Start", nodeGraph.StartState?.Name);
        }

        [Test]
        public void SimpleConnectionCreated()
        {
            var nodeGraph = new DummyNetwork();
            Assert.AreEqual("Start", nodeGraph.StartState.ToString());
            Assert.IsTrue(nodeGraph.IsValid());
        }
    }
}