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

        [Test(Description = "Should create a 1-to-1 (one-way) connection between start and end states")]
        public void SimpleConnectionCreated()
        {
            //Arrange & Act
            var nodeGraph = new TwoStateStartToEndNetwork();
            var connectionGroup = nodeGraph[nodeGraph.StartState];
            var stateDistributions = new List<StateDistribution>(connectionGroup.GetAllDistributions());
            var connectionWeight = stateDistributions[0][nodeGraph.EndTestState];

            //Assert
            Assert.AreEqual("Start", nodeGraph.StartTestState.ToString());
            Assert.AreEqual(1, stateDistributions.Count, "Should have only one connection");
            Assert.AreEqual(1, connectionWeight.GetWeight(null), "One connection should have a weight of 1");
            Assert.IsTrue(nodeGraph.IsValid());
        }
    }
}