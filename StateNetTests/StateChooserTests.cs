using NUnit.Framework;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests
{
    public class StateChooserTests
    {
        [Test]
        public void SetWeightTests()
        {
            var history = new List<State>();
            var nodeChooser = new StateChooser(history);

            var nodeConnection = new StateDistribution();
            Assert.AreEqual(0, nodeChooser.TotalWeight(nodeConnection));

            var d1 = new State("D1");
            var d2 = new State("D2");

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