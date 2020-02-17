using Aptacode.StateNet.Connections;
using NUnit.Framework;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests
{
    public class NodeChooserTests
    {
        [Test]
        public void SetWeightTests()
        {
            var history = new List<Node>();
            var nodeChooser = new NodeChooser(history);

            var nodeConnection = new NodeConnections();
            Assert.AreEqual(0, nodeChooser.TotalWeight(nodeConnection));

            var d1 = new Node("D1");
            var d2 = new Node("D2");

            nodeConnection.UpdateWeight(d1, 1);
            nodeConnection.UpdateWeight(d2, 2);

            Assert.AreEqual(3, nodeChooser.TotalWeight(nodeConnection));

            nodeConnection.UpdateWeight(d1, 2);
            nodeConnection.UpdateWeight(d2, 0);

            Assert.AreEqual(2, nodeChooser.TotalWeight(nodeConnection));
        }

        //TODO
        // ToString Test
        // GetNext probability distribution Tests
        // SetWeight new format
        // SetWeight Fluent API?
    }
}