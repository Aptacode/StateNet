using System.Collections.Generic;
using Aptacode.StateNet.Connections.Weights;
using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.ConnectionWeight
{
    public class StaticWeightTests
    {
        public static IEnumerable<TestCaseData> ChangingSetWeightTestCases
        {
            get
            {
                yield return new TestCaseData(0, 0);
                yield return new TestCaseData(-1, 0);
                yield return new TestCaseData(1, 1);
                yield return new TestCaseData(100, 100);
            }
        }

        public static IEnumerable<TestCaseData> ChangingWeightTestCases
        {
            get
            {
                yield return new TestCaseData(1, 0, 0);
                yield return new TestCaseData(1, -1, 0);
                yield return new TestCaseData(0, 1, 1);
                yield return new TestCaseData(-1, 1, 1);
                yield return new TestCaseData(1, 1, 1);
                yield return new TestCaseData(100, 1, 1);
            }
        }

        public static IEnumerable<TestCaseData> ChangingHistoryTestCases
        {
            get
            {
                yield return new TestCaseData(1, StateHistoryGenerator.Generate());
                yield return new TestCaseData(1, StateHistoryGenerator.Generate(1));
                yield return new TestCaseData(1, StateHistoryGenerator.Generate(1, 1));
                yield return new TestCaseData(1, StateHistoryGenerator.Generate(1, 2));
            }
        }

        [Test]
        [TestCaseSource(nameof(ChangingSetWeightTestCases))]
        public void GetWeight_Returns_ConstructorWeight(int setWeight, int expectedValue)
        {
            Assert.AreEqual(expectedValue, new StaticWeight(setWeight).GetWeight(null));
        }

        [Test]
        [TestCaseSource(nameof(ChangingHistoryTestCases))]
        public void SetWeight_IsNotAffectedBy_History(int setWeight, List<State> history)
        {
            Assert.AreEqual(setWeight, new StaticWeight(setWeight).GetWeight(history));
        }

        [Test]
        [TestCaseSource(nameof(ChangingWeightTestCases))]
        public void SetWeight_Overwrites_ConstructorWeight(int initialWeight, int setWeight, int expectedValue)
        {
            var connectionWeight = new StaticWeight(initialWeight);
            connectionWeight.Weight = setWeight;
            Assert.AreEqual(expectedValue, connectionWeight.GetWeight(null));
        }
    }
}