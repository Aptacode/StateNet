using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    /// <summary>
    ///     Checks if the various states in a network are being assigned to correctly.
    ///     Primary focus is testing that the *initial* Network reflection is behaving as expected.
    /// </summary>
    public class BasicAttributeAssignmentTests
    {
        [Test]
        public void StatesCreatedUsingFields()
        {
            var network = new TwoStateStartToEndNetwork();
            var states = new List<State>(network.GetAll());

            Assert.AreEqual(2, states.Count);
            Assert.AreEqual("Start", network.StartTestState.Name);
            Assert.AreEqual("End", network.EndTestState.Name);
        }

        [Test(Description =
            "Should find 3 states when instantiating a class that has 3 properties with State Attributes")]
        public void StatesCreatedUsingProperties()
        {
            var network = new TwoStatePropertyAttributeNetwork();
            var states = new List<State>(network.GetAll());

            Assert.AreEqual(3, states.Count);
            Assert.AreEqual("Start", network.StartTestState.Name);
            Assert.AreEqual("End", network.EndTestState.Name);
            //Deliberately not using GetAll("Private"), as (for now) it also changes the network  
            Assert.AreEqual(1, network.GetAll().Count(state => state.Name == "Private"));
        }

        [Test]
        public void IsStartStateSetByFields()
        {
            var network = new TwoStateStartToEndNetwork();

            Assert.AreEqual("Start", network.StartTestState?.Name);
        }

        [Test(Description = "Should have an assigned start state based on use of StartStateAttribute")]
        public void IsStartStateSetUsingProperties()
        {
            var network = new TwoStatePropertyAttributeNetwork();

            Assert.AreEqual("Start", network.StartTestState?.Name);
        }

        [Test(Description = "Should create a 1-to-1 (one-way) connection between start and end states")]
        public void SimpleConnectionCreatedByFields()
        {
            //Arrange & Act
            var network = new TwoStateStartToEndNetwork();
            var connectionGroup = network[network.StartState];
            var stateDistributions = new List<StateDistribution>(connectionGroup.GetAllDistributions());
            var connectionWeight = stateDistributions[0][network.EndTestState];

            //Assert
            Assert.AreEqual(1, stateDistributions.Count, "Should have only one connection");
            Assert.AreEqual(1, connectionWeight?.GetConnectionWeight(null), "One connection should have a weight of 1");
            Assert.IsTrue(network.IsValid());
        }

        [Test(Description = "Should create a 3-state, 2-step connection series (start-private-end)")]
        public void SimpleConnectionCreatedByProperties()
        {
            //Arrange & Act
            var network = new TwoStatePropertyAttributeNetwork();
            var privateState = network.GetState("Private");

            var firstConnectionGroup = network[network.StartState];
            var secondConnectionGroup = network[privateState];

            var firstStateDistributions = new List<StateDistribution>(firstConnectionGroup.GetAllDistributions());
            var secondStateDistributions = new List<StateDistribution>(secondConnectionGroup.GetAllDistributions());

            var firstConnectionWeight = firstStateDistributions[0][privateState];
            var secondConnectionWeight = secondStateDistributions[0][network.EndTestState];

            //Assert
            Assert.AreEqual(1, firstStateDistributions.Count, "Should have only one connection");
            Assert.AreEqual(1, firstConnectionWeight?.GetConnectionWeight(null),
                "One connection should have a weight of 1");
            Assert.AreEqual(1, secondStateDistributions.Count, "Should have only one connection");
            Assert.AreEqual(1, secondConnectionWeight?.GetConnectionWeight(null),
                "One connection should have a weight of 1");
            Assert.IsTrue(network.IsValid());
        }
    }
}