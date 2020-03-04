using System;
using System.Collections.Generic;
using Aptacode.StateNet.Random;
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
                yield return new TestCaseData(StateDistributionGenerator.Generate(), StateHistoryGenerator.Generate(), 0, "An empty distribution should return 0 weight");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1), StateHistoryGenerator.Generate(), 1, "Total Weight = 1");
                yield return new TestCaseData(StateDistributionGenerator.Generate(0,1), StateHistoryGenerator.Generate(), 1, "Total Weight = 1");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1,2), StateHistoryGenerator.Generate(), 3, "Total Weight = 3");
                yield return new TestCaseData(StateDistributionGenerator.Generate(1,-1), StateHistoryGenerator.Generate(), 1, "Negative weights should count as 0");
            }
        }
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(StateDistributionGenerator.Generate(), StateHistoryGenerator.Generate(), 0, "An empty distribution should return 0 weight");
            }
        }


        [Test]
        [TestCaseSource(nameof(NodeChooserTotalWeightTestCases))]
        public void NodeChooser_TotalWeight(StateDistribution stateDistribution, List<State> stateHistory, int expectedValue, string message = "")
        {
            Assert.AreEqual(expectedValue, new StateChooser(new SystemRandomNumberGenerator(), stateHistory).TotalWeight(stateDistribution), message);
        }

        [Test]
        [TestCaseSource(nameof(NodeChooserTotalWeightTestCases))]
        public void NodeChooser_ChooseValue(StateDistribution stateDistribution, List<State> stateHistory, int expectedValue, string message = "")
        {
            Assert.AreEqual(expectedValue, new StateChooser(new SystemRandomNumberGenerator(), stateHistory).TotalWeight(stateDistribution), message);
        }
    }
}