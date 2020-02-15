using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;


namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class NodeChooserCollectionTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Tests()
        {
            var nodeChooserCollection = new NodeChooserCollection();
            Assert.AreEqual(false, nodeChooserCollection.HasValidChoice);

            var chooser1 = new NodeChooser();
            var chooser2 = new NodeChooser();

            var d1 = new Node("D1");
            var d2 = new Node("D2");

            nodeChooserCollection["Next"] = chooser1;
            nodeChooserCollection["Next"] = chooser2;
            Assert.AreEqual(false, nodeChooserCollection.HasValidChoice);

            chooser1.SetWeight(d1, 0);

            Assert.AreEqual(false, nodeChooserCollection.HasValidChoice);

            chooser1.SetWeight(d2, 2);

            chooser2.SetWeight(d1, 2);
            chooser2.SetWeight(d2, 1);
            Assert.AreEqual(true, nodeChooserCollection.HasValidChoice);

        }

        //TODO ToString Test
        //TODO Next probability distrobution Tests
    }
}
