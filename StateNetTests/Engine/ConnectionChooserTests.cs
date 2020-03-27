using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Engine
{
    public class ConnectionChooserTests
    {
        public static IEnumerable<TestCaseData> RandomChoiceTestCases
        {
            get
            {
                yield return new TestCaseData(
                    DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "c", 1), ("c", "next", "d", 1)),
                    1, 0);
                yield return new TestCaseData(
                    DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "c", 1), ("c", "next", "d", 1)),
                    2, 1);
                yield return new TestCaseData(
                    DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "c", 1), ("c", "next", "d", 1)),
                    3, 2);
                yield return new TestCaseData(
                    DummyConnections.Generate(("a", "next", "b", 1), ("b", "next", "c", 2), ("c", "next", "d", 1)),
                    3, 1);
            }
        }
        
        [Test]
        [TestCaseSource(nameof(RandomChoiceTestCases))]
        public void NodeChooser_ChooseValue(IEnumerable<Connection> connectionDistribution,
            int weight,
            int expectedChoice)
        {
            var chooser =
                new ConnectionChooser(new DummyRandomNumberGenerator(weight), StateHistoryGenerator.Generate());

            var expectedConnection = connectionDistribution.ElementAt(expectedChoice);
            var actualConnection = chooser.Choose(connectionDistribution);
            Assert.AreEqual(expectedConnection, actualConnection, "Should choose the correct choice");
        }
    }
}