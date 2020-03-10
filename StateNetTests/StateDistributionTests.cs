using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class StateDistributionTests
    {
        public static IEnumerable<TestCaseData> StateDistributionIsValidTestCases
        {
            get
            {
                yield return new TestCaseData(StateDistributionGenerator.Generate(), false,
                    "An empty distribution has no connections");
                yield return new TestCaseData(StateDistributionGenerator.Generate(0), false,
                    "Has no valid connections");
                yield return new TestCaseData(StateDistributionGenerator.Generate(-1), false,
                    "Has no valid connections");

                yield return new TestCaseData(StateDistributionGenerator.Generate(1), true,
                    "Should have one connection");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 1), true,
                    "Should have two connections");
                yield return new TestCaseData(StateDistributionGenerator.Generate(0, 1), true,
                    "Has one valid connection");
            }
        }

        public static IEnumerable<TestCaseData> StateDistributionClearTestCases
        {
            get
            {
                yield return new TestCaseData(StateDistributionGenerator.Generate(), "Should have no connections");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1), "Should have no connections");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 1), "Should have no connections");
            }
        }

        public static IEnumerable<TestCaseData> StateDistributionAlwaysTestCases
        {
            get
            {
                yield return new TestCaseData(StateDistributionGenerator.Generate());
                yield return new TestCaseData(StateDistributionGenerator.Generate(1));
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 1));
            }
        }

        public static IEnumerable<TestCaseData> StateDistributionGetWeightsCases
        {
            get
            {
                yield return new TestCaseData(StateDistributionGenerator.Generate(-1), 0);
                yield return new TestCaseData(StateDistributionGenerator.Generate(), 0);
                yield return new TestCaseData(StateDistributionGenerator.Generate(1), 1);
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 1), 2);
            }
        }


        [Test]
        [TestCaseSource(nameof(StateDistributionIsValidTestCases))]
        public void StateDistribution_IsValid(StateDistribution stateDistribution, bool expectedValue,
            string message = "")
        {
            Assert.AreEqual(expectedValue, stateDistribution.HasConnections, message);
        }

        [Test]
        [TestCaseSource(nameof(StateDistributionClearTestCases))]
        public void StateDistribution_Clear(StateDistribution stateDistribution, string message = "")
        {
            stateDistribution.Clear();
            Assert.IsFalse(stateDistribution.HasConnections, message);
        }

        [Test]
        [TestCaseSource(nameof(StateDistributionAlwaysTestCases))]
        public void StateDistribution_Always(StateDistribution stateDistribution)
        {
            stateDistribution.Always(new State("1"));
            Assert.IsTrue(stateDistribution.HasConnections, "Should have 1 connection");
            Assert.AreEqual(1, stateDistribution.GetAll().Count, "Should have exactly 1 connection");
            Assert.AreEqual(1, stateDistribution.GetWeights().Sum(weight => weight.GetConnectionWeight(null)),
                "Sum of connection weights should be 0");
        }

        [Test]
        [TestCaseSource(nameof(StateDistributionGetWeightsCases))]
        public void StateDistribution_GetWeights(StateDistribution stateDistribution, int expectedSum)
        {
            Assert.AreEqual(expectedSum, stateDistribution.GetWeights().Sum(weight => weight.GetConnectionWeight(null)),
                $"Sum of connection weights should be {expectedSum}");
        }

        //[Test]
        //[TestCaseSource(nameof(StateDistributionGetWeightsCases))]
        //public void StateDistribution_GetWeights(StateDistribution stateDistribution, params (State, int)[] choices)
        //{
        //    stateDistribution.SetDistribution(choices);
        //    var actual = stateDistribution.GetAll().Select(pair => (pair.Key, pair.Value)).ToList();

        //    //Assert.IsTrue(actual.SequenceEqual<(State, int)>(choices.ToList()));
        //}
    }
}