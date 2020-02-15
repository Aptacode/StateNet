using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Attributes;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;
using System.Collections.Generic;


namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class AttributeBuilderTests
    {
        private class DummyGraph : NodeGraph
        {           
            [NodeConnection("U1", "U2", 1)]
            [NodeName("U1")]
            [NodeStart]
            public Node StartTestNode;

            [NodeName("U2")]
            public Node EndTestNode;
        }


        [SetUp]
        public void Setup() { }


        [Test]
        public void SimpleNodesCreated()
        {
            var nodeGraph = new DummyGraph();
            var nodes = new List<Node>(nodeGraph.GetAll());

            Assert.AreEqual(2, nodes.Count);
            Assert.AreEqual("U1", nodeGraph.StartTestNode.Name);
            Assert.AreEqual("U2", nodeGraph.EndTestNode.Name);
        }

        [Test]
        public void IsStartNodeSet()
        {
            var nodeGraph = new DummyGraph();

            Assert.AreEqual("U1", nodeGraph.StartNode?.Name);            
        }

        [Test]
        public void SimpleConnectionCreated()
        {
            var nodeGraph = new DummyGraph();

            Assert.AreEqual("U1->U2", nodeGraph.StartTestNode.ToString());
            Assert.IsTrue(nodeGraph.IsValid());
        }
    }
}
