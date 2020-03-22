using System.Collections.Generic;
using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.ConnectionWeight
{
    public class ConnectionWeightTests
    {
        public static IEnumerable<TestCaseData> StaticWeightFromStringTestCases
        {
            get
            {
                yield return new TestCaseData("0", StateHistoryGenerator.Generate(), 0);
                yield return new TestCaseData("-1", StateHistoryGenerator.Generate(), 0);
                yield return new TestCaseData("1", StateHistoryGenerator.Generate(), 1);
                yield return new TestCaseData("10", StateHistoryGenerator.Generate(), 10);
            }
        }

        public static IEnumerable<TestCaseData> InvalidDescription
        {
            get
            {
                yield return new TestCaseData("ten", StateHistoryGenerator.Generate(), 0);
                yield return new TestCaseData("1.5", StateHistoryGenerator.Generate(), 0);
            }
        }

        public static IEnumerable<TestCaseData> VisitCountWeightFromStringTestCases
        {
            get
            {
                yield return new TestCaseData("StateVisitCount(\"1\") > 1 ? 1 : 0", StateHistoryGenerator.Generate(2, 2), 0);
                yield return new TestCaseData("StateVisitCount(\"1\") > 1 ? 1 : 0", StateHistoryGenerator.Generate(1), 0);
                yield return new TestCaseData("StateVisitCount(\"1\") > 1 ? 1 : 0", StateHistoryGenerator.Generate(1, 1), 1);
            }
        }

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
            Assert.AreEqual(expectedValue, new Connections.ConnectionWeight(setWeight).GetWeight(null));
        }

        [Test]
        [TestCaseSource(nameof(ChangingHistoryTestCases))]
        public void SetWeight_IsNotAffectedBy_History(int setWeight, EngineLog log)
        {
            Assert.AreEqual(setWeight, new Connections.ConnectionWeight(setWeight).GetWeight(log));
        }

        [Test]
        [TestCaseSource(nameof(StaticWeightFromStringTestCases))]
        [TestCaseSource(nameof(VisitCountWeightFromStringTestCases))]
        [TestCaseSource(nameof(InvalidDescription))]
        public void ConnectionWeightParser_FromString_ReturnsExpectedWeight_GivenHistory(string input,
            EngineLog engineLog, int expectedWeight)
        {
            Assert.AreEqual(expectedWeight, new Connections.ConnectionWeight(input).GetWeight(engineLog));
        }
    }
}