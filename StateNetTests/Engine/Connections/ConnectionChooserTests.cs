using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine.Connections;
using Aptacode.StateNet.Network.Connections;
using Aptacode.StateNet.Tests.Helpers;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Engine.Connections
{
    public class ConnectionChooserTests
    {
        public static IEnumerable<TestCaseData> ChooseConnectionValidTestCases
        {
            get
            {
                yield return new TestCaseData(
                    DummyConnections.Create(
                        ("a", "next", "b", 1),
                        ("b", "next", "c", 1),
                        ("c", "next", "d", 1)),
                    1, 0);
                yield return new TestCaseData(
                    DummyConnections.Create(
                        ("a", "next", "b", 1),
                        ("b", "next", "c", 1),
                        ("c", "next", "d", 1)),
                    2, 1);
                yield return new TestCaseData(
                    DummyConnections.Create(
                        ("a", "next", "b", 1),
                        ("b", "next", "c", 1),
                        ("c", "next", "d", 1)),
                    3, 2);
                yield return new TestCaseData(
                    DummyConnections.Create(
                        ("a", "next", "b", 1),
                        ("b", "next", "c", 2),
                        ("c", "next", "d", 1)),
                    3, 1);
            }
        }

        /// <summary>
        ///     Tests that the ConnectionChooser, given a specific weight chooses the expected connection
        /// </summary>
        /// <param name="connectionDistribution"></param>
        /// <param name="weight"></param>
        /// <param name="expectedConnectionIndex"></param>
        [Test]
        [TestCaseSource(nameof(ChooseConnectionValidTestCases))]
        public void ChooseConnection(IEnumerable<Connection> connectionDistribution,
            int weight,
            int expectedConnectionIndex)
        {
            var chooser =
                new ConnectionChooser(new DummyRandomNumberGenerator(weight), StateHistoryGenerator.Generate());

            var expectedConnection = connectionDistribution.ElementAt(expectedConnectionIndex);
            var actualConnection = chooser.Choose(connectionDistribution);
            Assert.AreEqual(expectedConnection, actualConnection, "Should choose the correct choice");
        }

        [Test]
        public void NullOrEmptyConnectionList()
        {
            var chooser =
                new ConnectionChooser(new DummyRandomNumberGenerator(1), StateHistoryGenerator.Generate());

            Assert.IsNull(chooser.Choose(new List<Connection>()));
            Assert.IsNull(chooser.Choose(null));
        }
    }
}