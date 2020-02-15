using NUnit.Framework;
using System.Linq;

namespace Aptacode.StateNet.Tests
{
    public class Programatic_NodeGraphTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void GraphTests()
        {
            var graph = new NodeGraph();
           
            Assert.AreEqual(0, graph.GetAll().Count());
            Assert.AreEqual(0, graph.GetEndNodes().Count());
            Assert.AreEqual(null, graph.StartNode);
            Assert.AreEqual(false, graph.IsValid());

            var start = graph.GetNode("Start");
            var d1 = graph.GetNode("D1");
            var d2 = graph.GetNode("D2");
            var end = graph.GetNode("End");

            graph.StartNode = start;
            start["Next"].Distribution((d1, 1), (d2, 2));
            d1["Left"].Distribution((d1, 1), (end, 2));
            d2["Right"].Distribution((d1, 1), (end, 2));

            Assert.AreEqual(4, graph.GetAll().Count());
            Assert.AreEqual(1, graph.GetEndNodes().Count());
            Assert.AreEqual(start, graph.StartNode);
            Assert.AreEqual(true, graph.IsValid());
        }

        //TODO ToString Test
        //TODO GetNext probability distrobution Tests
    }
}
