using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    /// <summary>
    ///     Checks that a network declared using attributes maintains its
    ///     initial attribute-declared architecture, plus non-overriding user additions.
    ///     For example, adding extra states or state changes.
    /// </summary>
    public class ExtendingAttributeDrivenNetworksTests
    {
        [Test(Description = "Should have 3 states when extending 2 attribute-state network with new unconnected state")]
        public void TestAddingSingleNewState()
        {
            //Arrange
            var network = new TwoStateStartToEndNetwork();
            var states = new List<State>(network.GetStates());

            Assert.AreEqual(2, states.Count);
            Assert.AreEqual("Start", network.StartTestState.Name);
            Assert.AreEqual("End", network.EndTestState.Name);

            //Act
            var newState = network.GetState("NewState");

            //Assert
            Assert.AreEqual(3, network.GetStates().Count());
            Assert.AreEqual("NewState", newState.Name);
        }

        [Test(Description =
            "Should have an extra connection when extending 2 attribute-state network with new connected state")]
        public void TestAddingNewConnectedState()
        {
            //Arrange
            IStateNetwork stateNetwork = new TwoStateStartToEndNetwork();
            var states = new List<State>(stateNetwork.GetStates());

            //Act
            var newState = stateNetwork.GetState("NewState");

            //TODO - make use AddNewConnection, then make assertions. Only after checking if this type of runtime connection addition is desirable 

            //Assert
            Assert.Inconclusive("Test is missing main body and asserts. Needs to be completed.");
        }
    }
}