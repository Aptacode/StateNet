using System.Collections.Generic;
using Aptacode.StateNet.ConnectionWeight;
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
                yield return new TestCaseData(0);
                yield return new TestCaseData(-1);
                yield return new TestCaseData(1);
                yield return new TestCaseData(100);
            }
        }

        public static IEnumerable<TestCaseData> ChangingWeightTestCases
        {
            get
            {
                yield return new TestCaseData(0, 1);
                yield return new TestCaseData(-1, 1);
                yield return new TestCaseData(1, 1);
                yield return new TestCaseData(100, 1);
            }
        }

        public static IEnumerable<TestCaseData> ChangingHistoryTestCases
        {
            get
            {
                yield return new TestCaseData(1, StateHistoryGenerator.History());
                yield return new TestCaseData(1, StateHistoryGenerator.History(1));
                yield return new TestCaseData(1, StateHistoryGenerator.History(1, 1));
                yield return new TestCaseData(1, StateHistoryGenerator.History(1, 2));
            }
        }

        [Test]
        [TestCaseSource(nameof(ChangingSetWeightTestCases))]
        public void GetWeight_Returns_ConstructorWeight(int setWeight)
        {
            Assert.AreEqual(setWeight, new StaticWeight(setWeight).GetConnectionWeight(null));
        }

        [Test]
        [TestCaseSource(nameof(ChangingHistoryTestCases))]
        public void SetWeight_IsNotAffectedBy_History(int setWeight, List<State> history)
        {
            Assert.AreEqual(setWeight, new StaticWeight(setWeight).GetConnectionWeight(history));
        }

        [Test]
        [TestCaseSource(nameof(ChangingWeightTestCases))]
        public void SetWeight_Overwrites_ConstructorWeight(int initialWeight, int setWeight)
        {
            var connectionWeight = new StaticWeight(initialWeight);
            connectionWeight.Weight = setWeight;
            Assert.AreEqual(setWeight, connectionWeight.GetConnectionWeight(null));
        }
    }
}