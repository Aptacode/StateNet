using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Tests.Helpers;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Network
{
    public class StateNetworkEditorTests
    {
        #region States

        [Test]
        public void GetStates()
        {
            var network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);
            var stateA = networkEditor.CreateState("a");
            var stateB = networkEditor.CreateState("b");
            var stateC = networkEditor.CreateState("c");
            Assert.IsTrue(networkEditor.GetStates().Contains(stateA));
            Assert.IsTrue(networkEditor.GetStates().Contains(stateB));
            Assert.IsTrue(networkEditor.GetStates().Contains(stateC));
        }

        [Test]
        public void GetState_CreatesNewStateIfMissing()
        {
            var network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);
            var stateB = networkEditor.CreateState("b");
            var stateC = networkEditor.GetState("c");
            var stateD = networkEditor.GetState("d");
            var stateE = networkEditor.GetState("e", false);

            Assert.IsTrue(network.GetStates().Contains(stateB));
            Assert.IsTrue(network.GetStates().Contains(stateC));
            Assert.IsTrue(network.GetStates().Contains(stateD));
            Assert.IsFalse(network.GetStates().Contains(stateE));
        }

        [Test]
        public void GetState_ReturnsState()
        {
            var network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);

            var stateA = network.CreateState("a");

            Assert.AreEqual(stateA, networkEditor.CreateState("a"));
            Assert.AreEqual(stateA, networkEditor.GetState("a"));
            Assert.AreEqual(stateA, networkEditor.GetState("a"));
            Assert.AreEqual(stateA, networkEditor.GetState("a", false));
        }

        [Test]
        public void RemoveState()
        {
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1)));

            var networkEditor = new StateNetworkEditor(network);

            networkEditor.RemoveState("a");

            Assert.IsNull(network.GetState("a"));
            Assert.That(
                network.Connections.Where(connection => connection.Source.Name == "a" || connection.Target.Name == "a"),
                Is.Empty);
        }

        [Test]
        public void GetEndStates()
        {
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "b",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Generate(("b", "next", "c", 1)));

            var networkEditor = new StateNetworkEditor(network);


            var stateA = networkEditor.GetState("a");
            var stateB = networkEditor.GetState("b");
            var stateC = networkEditor.GetState("c");

            Assert.IsTrue(stateA.IsEnd());
            Assert.IsFalse(stateB.IsEnd());
            Assert.IsTrue(stateC.IsEnd());

            Assert.That(networkEditor.GetEndStates().Contains(stateA));
            Assert.That(!networkEditor.GetEndStates().Contains(stateB));
            Assert.That(networkEditor.GetEndStates().Contains(stateC));
        }

        #endregion

        #region Inputs

        [Test]
        public void GetInputs()
        {
            IStateNetwork network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);

            var inputA = network.CreateInput("a");
            var inputB = network.CreateInput("b");
            var inputC = network.CreateInput("c");

            Assert.IsTrue(networkEditor.GetInputs().Contains(inputA));
            Assert.IsTrue(networkEditor.GetInputs().Contains(inputB));
            Assert.IsTrue(networkEditor.GetInputs().Contains(inputC));
        }

        [Test]
        public void GetInputs_CreatesNewInputIfMissing()
        {
            IStateNetwork network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);

            var InputA = networkEditor.CreateInput("b");
            var InputB = networkEditor.GetInput("c");
            var InputC = networkEditor.GetInput("d");
            var InputD = networkEditor.GetInput("e", false);

            Assert.IsTrue(network.GetInputs().Contains(InputA));
            Assert.IsTrue(network.GetInputs().Contains(InputB));
            Assert.IsTrue(network.GetInputs().Contains(InputC));
            Assert.IsFalse(network.GetInputs().Contains(InputD));
        }

        [Test]
        public void GetInputs_ReturnsInput()
        {
            IStateNetwork network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);

            var InputA = network.CreateInput("a");

            Assert.AreEqual(InputA, networkEditor.CreateInput("a"));
            Assert.AreEqual(InputA, networkEditor.GetInput("a"));
            Assert.AreEqual(InputA, networkEditor.GetInput("a"));
            Assert.AreEqual(InputA, networkEditor.GetInput("a", false));
        }

        [Test]
        public void RemoveInput()
        {
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1)));
            var networkEditor = new StateNetworkEditor(network);

            networkEditor.RemoveInput("next");

            Assert.IsNull(network.GetInput("next"));
            Assert.That(network.Connections.Where(connection => connection.Input.Name == "next"), Is.Empty);
        }

        #endregion

        #region Connections

        [Test]
        public void GetConnections_DoesNotCreateNewStateOrInput()
        {
            IStateNetwork network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);

            var connections = networkEditor.GetConnections("a", "next");

            Assert.AreEqual(0, connections.Count());
            Assert.AreEqual(0, network.GetStates().Count());
            Assert.AreEqual(0, network.GetInputs().Count());
        }

        [Test]
        public void GetConnections_ReturnsAllConnections()
        {
            var allConnections =
                DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1));
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                allConnections);
            var networkEditor = new StateNetworkEditor(network);


            Assert.That(allConnections, Is.SupersetOf(networkEditor.GetConnections()));
            Assert.That(allConnections, Is.SubsetOf(networkEditor.GetConnections()));

            Assert.That(allConnections, Is.SupersetOf(networkEditor.GetConnections()));
            Assert.That(allConnections, Is.SubsetOf(networkEditor.GetConnections()));
        }

        [Test]
        public void GetConnections_ReturnsCorrectConnections()
        {
            var allConnections =
                DummyConnections.Generate(
                    ("a", "back", "a", 1),
                    ("a", "next", "b", 1),
                    ("a", "next", "c", 1),
                    ("b", "back", "a", 1));

            //Create a network with 3 states (a, b, c), 1 input (next) and three connections
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next"),
                allConnections);

            var networkEditor = new StateNetworkEditor(network);


            var ExpectedStateFilteredConnections = allConnections.Where(connection => connection.Source.Name == "a");
            var ActualstateFilteredConnections = networkEditor.GetConnections("a");

            Assert.That(ExpectedStateFilteredConnections, Is.SupersetOf(ActualstateFilteredConnections));
            Assert.That(ExpectedStateFilteredConnections, Is.SubsetOf(ActualstateFilteredConnections));


            var ExpectedStateAndInputFilteredConnections = allConnections.Where(connection =>
                connection.Source.Name == "a" && connection.Input.Name == "next");
            var ActualstateAndInputFilteredConnections = networkEditor.GetConnections("a", "next");

            Assert.That(ExpectedStateAndInputFilteredConnections,
                Is.SupersetOf(ActualstateAndInputFilteredConnections));
            Assert.That(ExpectedStateAndInputFilteredConnections, Is.SubsetOf(ActualstateAndInputFilteredConnections));

            var expectedConnections = allConnections.Where(connection =>
                connection.Source.Name == "a" && connection.Input.Name == "next" && connection.Target.Name == "b");
            var actualConnection = networkEditor.GetConnection("a", "next", "b");

            Assert.That(expectedConnections.Contains(actualConnection));
        }

        #endregion
    }
}