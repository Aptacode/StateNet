using System.Collections.Generic;
using Aptacode.StateNet.Engine.Connections;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Engine
{
    public class ConnectionDistributionTests
    {
        public static IEnumerable<TestCaseData> SumWeightsTestCases
        {
            get
            {
                yield return new TestCaseData(DummyConnections.CreateDistribution(), 0,
                    "An empty distribution should return 0 weight");
                yield return new TestCaseData(DummyConnections.CreateDistribution(("a", "next", "b", 1)), 1,
                    "Total Weight = 1");
                yield return new TestCaseData(
                    DummyConnections.CreateDistribution(("a", "next", "b", 0), ("b", "next", "a", 1)), 1,
                    "Total Weight = 1");
                yield return new TestCaseData(
                    DummyConnections.CreateDistribution(("a", "next", "b", 1), ("b", "next", "a", 2)), 3,
                    "Total Weight = 3");
                yield return new TestCaseData(DummyConnections.CreateDistribution(("a", "next", "b", -1)), 0,
                    "Negative weights should count as 0");
            }
        }

        [Test]
        [TestCaseSource(nameof(SumWeightsTestCases))]
        public void SumWeights(ConnectionDistribution connectionDistribution, int expectedValue, string message)
        {
            Assert.AreEqual(expectedValue, connectionDistribution.SumWeights(), message);
        }
    }
}