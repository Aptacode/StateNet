using NUnit.Framework;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    /// <summary>
    /// Checks if the various states in a network are being assigned to correctly.
    /// Primary focus is testing that the *initial* Network reflection is behaving as expected.
    /// </summary>
    public class BasicAttributeAssignmentTests
    {
        [Test]
        public void StatesCreated()
        {
            var network = new TwoStateStartToEndNetwork();
            var states = new List<State>(network.GetAll());

            Assert.AreEqual(2, states.Count);
            Assert.AreEqual("Start", network.StartTestState.Name);
            Assert.AreEqual("End", network.EndTestState.Name);
        }

        [Test]
        public void IsStartStateSet()
        {
            var nodeGraph = new TwoStateStartToEndNetwork();

            Assert.AreEqual("Start", nodeGraph.StartTestState?.Name);
        }

        [Test]
        public void SimpleConnectionCreated()
        {
            var nodeGraph = new TwoStateStartToEndNetwork();
            Assert.AreEqual("Start", nodeGraph.StartTestState.ToString());
            Assert.IsTrue(nodeGraph.IsValid());
        }
    }
}