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
        [Test]
        public void CanCreateA()
        {
            var graph = new Network();

            Assert.AreEqual(0, graph.GetStates().Count());
            Assert.AreEqual(0, graph.GetEndStates().Count());
            Assert.AreEqual(null, graph.StartState);
            Assert.AreEqual(false, graph.IsValid());

            var start = graph["Start"];
            var d1 = graph["D1"];
            var d2 = graph["D2"];
            var end = graph["End"];

            graph.SetStart(start);
            graph.SetDistribution("Start", "Next", (d1, 1), (d2, 2));
            graph.SetDistribution("D1", "Left", (d1, 1), (end, 2));
            graph.SetDistribution("D2", "Right", (d1, 1), (end, 2));

            Assert.AreEqual(4, graph.GetStates().Count());
            Assert.AreEqual(1, graph.GetEndStates().Count());
            Assert.AreEqual(start, graph.StartState);
            Assert.AreEqual(true, graph.IsValid());
        }

        //TODO ToString Test
        //TODO GetNext probability distribution Tests
    }
}