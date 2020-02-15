using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Attributes;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;
using System.Collections.Generic;


namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class Attribute_NodeGraphTests
    {
        private class DummyGraph : NodeGraph
        {
            [NodeStart("Start")]
            [NodeConnection("Next", "End")]
            public Node StartTestNode;

            [NodeName("End")]
            public Node EndTestNode;
        }


        [SetUp]
        public void Setup() { }


        [Test]
        public void NodesCreated()
        {
            var nodeGraph = new DummyGraph();
            var nodes = new List<Node>(nodeGraph.GetAll());

            Assert.AreEqual(2, nodes.Count);
            Assert.AreEqual("Start", nodeGraph.StartTestNode.Name);
            Assert.AreEqual("End", nodeGraph.EndTestNode.Name);
        }

        [Test]
        public void IsStartNodeSet()
        {
            var nodeGraph = new DummyGraph();

            Assert.AreEqual("Start", nodeGraph.StartNode?.Name);            
        }

        [Test]
        public void SimpleConnectionCreated()
        {
            var nodeGraph = new DummyGraph();
            Assert.AreEqual("Start:(Next->(End:1))", nodeGraph.StartTestNode.ToString());
            Assert.IsTrue(nodeGraph.IsValid());
        }
    }
}
