using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Tests.Helpers;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Network
{
    public class ProgrammaticNetworkTests
    {
        public static IEnumerable<TestCaseData> NetworkCreationTestCases
        {
            get
            {
                yield return new TestCaseData("", DummyStates.Create(), DummyInputs.Create(),
                    DummyConnections.Generate(), 0, false);
                yield return new TestCaseData("", DummyStates.Create("a"), DummyInputs.Create("next"),
                    DummyConnections.Generate(), 1, false);
                yield return new TestCaseData("", DummyStates.Create("a", "b"), DummyInputs.Create("next", "back"),
                    DummyConnections.Generate(), 2, false);
                yield return new TestCaseData("a", DummyStates.Create("a", "b"), DummyInputs.Create("next", "back"),
                    DummyConnections.Generate(("a", "next", "b", 1)), 1, true);
                yield return new TestCaseData("a", DummyStates.Create("a", "b"), DummyInputs.Create("next", "back"),
                    DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "a", 1)), 0, true);
            }
        }

        [Test]
        [TestCaseSource(nameof(NetworkCreationTestCases))]
        public void NetworkCreationTests(string startState, IEnumerable<State> states, IEnumerable<Input> inputs,
            IEnumerable<Connection> connections, int endStateCount, bool isValid)
        {
            var network = DummyProgrammaticNetworks.CreateNetwork(startState, states, inputs, connections);

            Assert.That(states, Is.SupersetOf(network.GetStates()));
            Assert.That(states, Is.SubsetOf(network.GetStates()));

            Assert.That(inputs, Is.SupersetOf(network.GetInputs()));
            Assert.That(inputs, Is.SubsetOf(network.GetInputs()));

            Assert.That(connections, Is.SupersetOf(network.GetConnections()));
            Assert.That(connections, Is.SubsetOf(network.GetConnections()));

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
            var stateA = network["a"];
            var stateB = network.CreateState("b");
            var stateC = network.GetState("c");
            var stateD = network.GetState("d");
            var stateE = network.GetState("e", false);

            Assert.IsTrue(network.GetStates().Contains(stateA));
            Assert.IsTrue(network.GetStates().Contains(stateB));
            Assert.IsTrue(network.GetStates().Contains(stateC));
            Assert.IsTrue(network.GetStates().Contains(stateD));
            Assert.IsFalse(network.GetStates().Contains(stateE));
        }

        [Test]
        public void GetState_ReturnsState()
        {
            IStateNetwork network = new StateNetwork();
            var stateA = network.CreateState("a");

            Assert.AreEqual(stateA, network["a"]);
            Assert.AreEqual(stateA, network.CreateState("a"));
            Assert.AreEqual(stateA, network.GetState("a"));
            Assert.AreEqual(stateA, network.GetState("a"));
            Assert.AreEqual(stateA, network.GetState("a", false));
        }

        [Test]
        public void RemoveState()
        {
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1)));

            network.RemoveState("a");

            Assert.IsNull(network.GetState("a", false));
            Assert.That(
                network.Connections.Where(connection => connection.From.Name == "a" || connection.To.Name == "a"),
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
            var InputA = network.CreateInput("b");
            var InputB = network.GetInput("c");
            var InputC = network.GetInput("d");
            var InputD = network.GetInput("e", false);

            Assert.IsTrue(network.GetInputs().Contains(InputA));
            Assert.IsTrue(network.GetInputs().Contains(InputB));
            Assert.IsTrue(network.GetInputs().Contains(InputC));
            Assert.IsFalse(network.GetInputs().Contains(InputD));
        }

        [Test]
        public void GetInputs_ReturnsInput()
        {
            IStateNetwork network = new StateNetwork();
            var InputA = network.CreateInput("a");

            Assert.AreEqual(InputA, network.CreateInput("a"));
            Assert.AreEqual(InputA, network.GetInput("a"));
            Assert.AreEqual(InputA, network.GetInput("a"));
            Assert.AreEqual(InputA, network.GetInput("a", false));
        }

        [Test]
        public void RemoveInput()
        {
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1)));

            network.RemoveInput("next");

            Assert.IsNull(network.GetInput("next", false));
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
                DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "a", 1), ("b", "back", "c", 1));
            var network = DummyProgrammaticNetworks.CreateNetwork(
                "a",
                DummyStates.Create("a", "b", "c"),
                DummyInputs.Create("next", "back"),
                allConnections);

            Assert.That(allConnections, Is.SupersetOf(network.Connections));
            Assert.That(allConnections, Is.SubsetOf(network.Connections));

            Assert.That(allConnections, Is.SupersetOf(network.GetConnections()));
            Assert.That(allConnections, Is.SubsetOf(network.GetConnections()));
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

            var ExpectedStateFilteredConnections = allConnections.Where(connection => connection.From.Name == "a");
            var ActualstateFilteredConnections = network.GetConnections("a");

            Assert.That(ExpectedStateFilteredConnections, Is.SupersetOf(ActualstateFilteredConnections));
            Assert.That(ExpectedStateFilteredConnections, Is.SubsetOf(ActualstateFilteredConnections));


            var ExpectedStateAndInputFilteredConnections = allConnections.Where(connection =>
                connection.From.Name == "a" && connection.Input.Name == "next");
            var ActualstateAndInputFilteredConnections = network.GetConnections("a", "next");

            Assert.That(ExpectedStateAndInputFilteredConnections,
                Is.SupersetOf(ActualstateAndInputFilteredConnections));
            Assert.That(ExpectedStateAndInputFilteredConnections, Is.SubsetOf(ActualstateAndInputFilteredConnections));

            var expectedConnections = allConnections.Where(connection =>
                connection.From.Name == "a" && connection.Input.Name == "next" && connection.To.Name == "b");
            var actualConnection = network.GetConnection("a", "next", "b");

            Assert.That(expectedConnections.Contains(actualConnection));
        }

        #endregion
    }
}