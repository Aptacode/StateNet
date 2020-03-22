using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public static class DummyProgrammaticNetworks
    {
        public static Network CreateEmptyNetwork()
        {
            return new Network();
        }

        public static Network CreateSingleStateNetwork()
        {
            var network = new Network();
            var state0 = new State("0");
            network.SetStart(state0);
            return network;
        }

        public static Network CreateSingleConnectionNetwork()
        {
            var state0 = new State("0");
            var state1 = new State("1");
            var network = new Network();

            network.SetStart(state0);
            network.Always(state0, "next", state1);

            return network;
        }

        public static Network CreateSelfConnectionNetwork()
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
            get { yield return new TestCaseData(DummyProgrammaticNetworks.CreateEmptyNetwork(), 0, 0, 0, 0, false); }
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


        //TODO ToString Test
        //TODO GetNext probability distribution Tests
    }
}