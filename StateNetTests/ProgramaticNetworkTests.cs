using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public static class DummyProgrammaticNetworks
    {
        public static INetwork CreateEmptyNetwork()
        {
            return new Network();
        }

        public static INetwork CreateSingleStateNetwork()
        {
            INetwork network = new Network();
            var state0 = new State("0");
            network.SetStart(state0);
            return network;
        }

        public static INetwork CreateSingleConnectionNetwork()
        {
            var state0 = new State("0");
            var state1 = new State("1");
            var network = new Network();

            network.SetStart(state0);
            network.Always(state0, "next", state1);

            return network;
        }

        public static INetwork CreateSelfConnectionNetwork()
        {
            var state0 = new State("0");
            var state1 = new State("1");
            var state2 = new State("2");

            var network = new Network();

            network.SetStart(state0);
            network.Connect(state0, "next", state1);
            network.Connect(state1, "next", state1);
            network.Connect(state1, "next", state2);

            return network;
        }
    }

    public class ProgrammaticNetworkTests
    {
        public static IEnumerable<TestCaseData> ProgrammaticNetworkCreationTestCases
        {
            get
            {
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateEmptyNetwork(), 0, 0, 0, 0, false);
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateSingleStateNetwork(), 1, 0, 0, 1, true);
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateSingleConnectionNetwork(), 2, 1, 1, 1,
                    true);
                yield return new TestCaseData(DummyProgrammaticNetworks.CreateSelfConnectionNetwork(), 3, 1, 3, 1,
                    true);
            }
        }

        [Test]
        [TestCaseSource(nameof(ProgrammaticNetworkCreationTestCases))]
        public void ProgrammaticNetworkCreation(Network network, int states, int inputs, int connections, int endStates,
            bool isValid)
        {
            Assert.AreEqual(states, network.GetStates().Count());
            Assert.AreEqual(inputs, network.GetInputs().Count());
            Assert.AreEqual(connections, network.GetConnections().Count());
            Assert.AreEqual(endStates, network.GetEndStates().Count());
            Assert.AreEqual(isValid, network.IsValid());
        }
    }
}