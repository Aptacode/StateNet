using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class ProgrammaticNetworkTests
    {
        public static IEnumerable<TestCaseData> ProgrammaticNetworkCreationTestCases
        {
            get
            {
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateNetwork("", DummyStates.Create(0), DummyActions.Create(0)), 0, 0, 0, 0, false);
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateNetwork("", DummyStates.Create(1), DummyActions.Create(1)), 1, 1, 0, 1, false);
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateNetwork("", DummyStates.Create(2), DummyActions.Create(2)), 2, 2, 0, 2, false);
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateNetwork("0", DummyStates.Create(2), DummyActions.Create(2), ("0", "0", "1", 1)), 2, 2, 1, 1, true);
            }
        }

        [Test]
        [TestCaseSource(nameof(ProgrammaticNetworkCreationTestCases))]
        public void NetworkCreationTests(StateNetwork network, int states, int inputs, int connections,
            int endStates,
            bool isValid)
        {
            Assert.AreEqual(states, network.GetStates().Count());
            Assert.AreEqual(inputs, network.GetInputs().Count());
            Assert.AreEqual(connections, network.GetConnections().Count());
            Assert.AreEqual(endStates, network.GetEndStates().Count());
            Assert.AreEqual(isValid, network.IsValid());
        }


        [Test]
        public void GetConnectionDoesNotCreateANewStateOrInputIfMissing()
        {
            IStateNetwork network = new StateNetwork();
            var connections = network["a", "next"];

            Assert.AreEqual(0, connections.Count());
            Assert.AreEqual(0, network.GetStates().Count());
            Assert.AreEqual(0, network.GetInputs().Count());

            connections = network.GetConnections("a", "next");

            Assert.AreEqual(0, connections.Count());
            Assert.AreEqual(0, network.GetStates().Count());
            Assert.AreEqual(0, network.GetInputs().Count());
        }

        [Test]
        public void GetConnectionReturnsCorrectConnection()
        {
            IStateNetwork network = DummyProgrammaticNetworks.CreateNetwork(
                "0",
                DummyStates.Create(2),
                DummyActions.Create(2), 
                ("0", "0", "1", 1));

            var connections = network["0", "0"];

            Assert.AreEqual(1, connections.Count());
            Assert.AreEqual("0", connections.First().From.Name);
            Assert.AreEqual("0", connections.First().Input.Name);
            Assert.AreEqual("1", connections.First().To.Name);
            Assert.AreEqual(1, connections.First().ConnectionWeight.Evaluate(null));
        }

    }
}