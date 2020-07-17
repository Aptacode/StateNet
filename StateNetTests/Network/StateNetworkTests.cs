using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Network
{
    public class StateNetworkTests
    {
        public static IEnumerable<TestCaseData> NetworkCreationTestCases
        {
            get
            {
                yield return new TestCaseData("", DummyStates.Create(), DummyInputs.Create(),
                    DummyConnections.Create(), 0, false);
                yield return new TestCaseData("", DummyStates.Create("a"), DummyInputs.Create("next"),
                    DummyConnections.Create(), 1, false);
                yield return new TestCaseData("", DummyStates.Create("a", "b"), DummyInputs.Create("next", "back"),
                    DummyConnections.Create(), 2, false);
                yield return new TestCaseData("a", DummyStates.Create("a", "b"), DummyInputs.Create("next", "back"),
                    DummyConnections.Create(("a", "next", "b", 1)), 1, true);
                yield return new TestCaseData("a", DummyStates.Create("a", "b"), DummyInputs.Create("next", "back"),
                    DummyConnections.Create(("a", "next", "b", 1), ("b", "next", "a", 1)), 0, true);
            }
        }

        [Test]
        [TestCaseSource(nameof(NetworkCreationTestCases))]
        public void NetworkCreationTests(string startState, IEnumerable<State> states, IEnumerable<Input> inputs,
            IEnumerable<StateNet.Network.Connections.Connection> connections, int endStateCount, bool isValid)
        {
            var network = DummyProgrammaticNetworks.Create(startState, states, inputs, connections);

            Assert.That(states, Is.SupersetOf(network.GetStates()));
            Assert.That(states, Is.SubsetOf(network.GetStates()));

            Assert.That(inputs, Is.SupersetOf(network.GetInputs()));
            Assert.That(inputs, Is.SubsetOf(network.GetInputs()));

            Assert.That(connections.Select(c => c.ToString()), Is.SupersetOf(network.GetConnections().Select(c => c.ToString())));
            Assert.That(connections.Select(c => c.ToString()), Is.SubsetOf(network.GetConnections().Select(c => c.ToString())));

            Assert.AreEqual(endStateCount, network.GetEndStates().Count());
            Assert.AreEqual(isValid, network.IsValid());
        }

        #region States

        [Test]
        public void GetStates()
        {
            IStateNetwork network = new StateNetwork();
            var stateA = network.CreateState("a");
            var stateB = network.CreateState("b");
            var stateC = network.CreateState("c");
            Assert.IsTrue(network.GetStates().Contains(stateA));
            Assert.IsTrue(network.GetStates().Contains(stateB));
            Assert.IsTrue(network.GetStates().Contains(stateC));
        }

        [Test]
        public void GetState_CreatesNewStateIfMissing()
        {
            IStateNetwork network = new StateNetwork();
            var stateA = network.GetState("a");
            var stateB = network.CreateState("a");
            var stateC = network.GetState("a");

            Assert.IsNull(stateA);
            Assert.IsTrue(network.GetStates().Contains(stateB));
            Assert.AreEqual(stateB, stateC);
        }

        [Test]
        public void GetState_ReturnsState()
        {
            IStateNetwork network = new StateNetwork();
            var stateA = network.CreateState("a");

            Assert.AreEqual(stateA, network.CreateState("a"));
            Assert.AreEqual(stateA, network.GetState("a"));
            Assert.AreEqual(stateA, network.GetState("a"));
        }

        [Test]
        public void RemoveState()
        {
            var network = DummyProgrammaticNetworks.Create(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Create(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1)));

            network.RemoveState("a");

            Assert.IsNull(network.GetState("a"));
            Assert.That(
                network.Connections.Where(connection => connection.Source.Name == "a" || connection.Target.Name == "a"),
                Is.Empty);
        }

        [Test]
        public void GetEndStates()
        {
            var network = DummyProgrammaticNetworks.Create(
                "b",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Create(("b", "next", "c", 1)));

            var stateA = network.GetState("a");
            var stateB = network.GetState("b");
            var stateC = network.GetState("c");

            Assert.IsTrue(stateA.IsEnd());
            Assert.IsFalse(stateB.IsEnd());
            Assert.IsTrue(stateC.IsEnd());

            Assert.That(network.GetEndStates().Contains(stateA));
            Assert.That(!network.GetEndStates().Contains(stateB));
            Assert.That(network.GetEndStates().Contains(stateC));
        }

        #endregion

        #region Inputs

        [Test]
        public void GetInputs()
        {
            IStateNetwork network = new StateNetwork();
            var inputA = network.CreateInput("a");
            var inputB = network.CreateInput("b");
            var inputC = network.CreateInput("c");

            Assert.IsTrue(network.GetInputs().Contains(inputA));
            Assert.IsTrue(network.GetInputs().Contains(inputB));
            Assert.IsTrue(network.GetInputs().Contains(inputC));
        }

        [Test]
        public void GetInputs_CreatesNewInputIfMissing()
        {
            IStateNetwork network = new StateNetwork();

            var InputA = network.GetInput("a");
            var InputB = network.CreateInput("a");
            var InputC = network.GetInput("a");

            Assert.IsNull(InputA);
            Assert.IsTrue(network.GetInputs().Contains(InputB));
            Assert.AreEqual(InputB, InputC);
        }

        [Test]
        public void GetInputs_ReturnsInput()
        {
            IStateNetwork network = new StateNetwork();
            var InputA = network.CreateInput("a");

            Assert.AreEqual(InputA, network.CreateInput("a"));
            Assert.AreEqual(InputA, network.GetInput("a"));
            Assert.AreEqual(InputA, network.GetInput("a"));
        }

        [Test]
        public void RemoveInput()
        {
            var network = DummyProgrammaticNetworks.Create(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Create(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1)));

            network.RemoveInput("next");

            Assert.IsNull(network.GetInput("next"));
            Assert.That(network.Connections.Where(connection => connection.Input.Name == "next"), Is.Empty);
        }

        #endregion

        #region Connections

        [Test]
        public void GetConnections_DoesNotCreateNewStateOrInput()
        {
            IStateNetwork network = new StateNetwork();
            var connections = network.GetConnections("a", "next");

            Assert.AreEqual(0, connections.Count());
            Assert.AreEqual(0, network.GetStates().Count());
            Assert.AreEqual(0, network.GetInputs().Count());
        }

        [Test]
        public void GetConnections_ReturnsAllConnections()
        {
            var allConnections =
                DummyConnections.Create(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1));
            var network = DummyProgrammaticNetworks.Create(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                allConnections);

            Assert.That(allConnections.Select(c => c.ToString()), Is.SupersetOf(network.Connections.Select(c => c.ToString())));
            Assert.That(allConnections.Select(c => c.ToString()), Is.SubsetOf(network.Connections.Select(c => c.ToString())));

            Assert.That(allConnections.Select(c => c.ToString()), Is.SupersetOf(network.GetConnections().Select(c => c.ToString())));
            Assert.That(allConnections.Select(c => c.ToString()), Is.SubsetOf(network.GetConnections().Select(c => c.ToString())));
        }

        [Test]
        public void GetConnections_ReturnsCorrectConnections()
        {
            var allConnections =
                DummyConnections.Create(
                    ("a", "back", "a", 1),
                    ("a", "next", "b", 1),
                    ("a", "next", "c", 1),
                    ("b", "back", "a", 1));

            //Create a network with 3 states (a, b, c), 1 input (next) and three connections
            var network = DummyProgrammaticNetworks.Create(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next"),
                allConnections);

            var ExpectedStateFilteredConnections = allConnections.Where(connection => connection.Source.Name == "a");
            var ActualstateFilteredConnections = network.GetConnections("a");

            Assert.That(ExpectedStateFilteredConnections.Select(c => c.ToString()), Is.SupersetOf(ActualstateFilteredConnections.Select(c => c.ToString())));
            Assert.That(ExpectedStateFilteredConnections.Select(c => c.ToString()), Is.SubsetOf(ActualstateFilteredConnections.Select(c => c.ToString())));


            var ExpectedStateAndInputFilteredConnections = allConnections.Where(connection =>
                connection.Source.Name == "a" && connection.Input.Name == "next");
            var ActualstateAndInputFilteredConnections = network.GetConnections("a", "next");

            Assert.That(ExpectedStateAndInputFilteredConnections.Select(c => c.ToString()),
                Is.SupersetOf(ActualstateAndInputFilteredConnections.Select(c => c.ToString())));
            Assert.That(ExpectedStateAndInputFilteredConnections.Select(c => c.ToString()), Is.SubsetOf(ActualstateAndInputFilteredConnections.Select(c => c.ToString())));

            var expectedConnections = allConnections.Where(connection =>
                connection.Source.Name == "a" && connection.Input.Name == "next" && connection.Target.Name == "b");
            var actualConnection = network.GetConnection("a", "next", "b");

            Assert.That(expectedConnections.Select(c => c.ToString()).Contains(actualConnection.ToString()));
        }

        #endregion
    }
}