using NUnit.Framework;
using System.Linq;

namespace Aptacode.StateNet.Tests
{
    public class ProgramaticNetworkTests
    {
        [Test]
        public void GraphTests()
        {
            var graph = new Network();

            Assert.AreEqual(0, graph.GetAll().Count());
            Assert.AreEqual(0, graph.GetEndStates().Count());
            Assert.AreEqual(null, graph.StartState);
            Assert.AreEqual(false, graph.IsValid());

            var start = graph["Start"];
            var d1 = graph["D1"];
            var d2 = graph["D2"];
            var end = graph["End"];

            graph.StartState = start;
            graph["Start", "Next"].SetDistribution((d1, 1), (d2, 2));
            graph["D1", "Left"].SetDistribution((d1, 1), (end, 2));
            graph["D2", "Right"].SetDistribution((d1, 1), (end, 2));

            Assert.AreEqual(4, graph.GetAll().Count());
            Assert.AreEqual(1, graph.GetEndStates().Count());
            Assert.AreEqual(start, graph.StartState);
            Assert.AreEqual(true, graph.IsValid());
        }

        //TODO ToString Test
        //TODO GetNext probability distribution Tests
    }
}