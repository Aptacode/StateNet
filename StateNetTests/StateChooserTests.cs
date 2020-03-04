using System.Collections.Generic;
using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class StateChooserTests
    {
        public static IEnumerable<TestCaseData> NodeChooserTotalWeightTestCases
        {
            get
            {
                yield return new TestCaseData(StateDistributionGenerator.Generate(), 0,
                    "An empty distribution should return 0 weight");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1), 1, "Total Weight = 1");
                yield return new TestCaseData(StateDistributionGenerator.Generate(0, 1), 1, "Total Weight = 1");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 2), 3, "Total Weight = 3");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, -1), 1,
                    "Negative weights should count as 0");
            }
        }

        public static IEnumerable<TestCaseData> NodeChooserChoiceTestCases
        {
            get
            {
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 1, 1), 1, 0);
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 1, 1), 2, 1);
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 1, 1), 3, 2);
                yield return new TestCaseData(StateDistributionGenerator.Generate(1, 2, 1), 3, 1);
            }
        }

        [Test]
        [TestCaseSource(nameof(NodeChooserTotalWeightTestCases))]
        public void NodeChooser_TotalWeight(StateDistribution stateDistribution, int expectedValue, string message = "")
        {
            Assert.AreEqual(expectedValue,
                new StateChooser(new DummyRandomNumberGenerator(), StateHistoryGenerator.Generate()).TotalWeight(
                    stateDistribution), message);
        }


        [Test]
        [TestCaseSource(nameof(NodeChooserChoiceTestCases))]
        public void NodeChooser_ChooseValue(StateDistribution stateDistribution, int weight, int expectedChoice)
        {
            Assert.AreEqual(expectedChoice.ToString(),
                new StateChooser(new DummyRandomNumberGenerator(weight), StateHistoryGenerator.Generate())
                    .Choose(stateDistribution).Name, "Should choose the correct choice");
        }
    }
}