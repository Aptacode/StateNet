using System.Collections.Generic;
using Aptacode.StateNet.ConnectionWeight;
using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.ConnectionWeight
{
    public class ConnectionWeightParserTests
    {
        public static IEnumerable<TestCaseData> StaticWeightFromStringTestCases
        {
            get
            {
                yield return new TestCaseData("StaticWeight:0", StateHistoryGenerator.Generate(), 0);
                yield return new TestCaseData("StaticWeight:-1", StateHistoryGenerator.Generate(), 0);
                yield return new TestCaseData("StaticWeight:1", StateHistoryGenerator.Generate(), 1);
                yield return new TestCaseData("StaticWeight:10", StateHistoryGenerator.Generate(), 10);
                yield return new TestCaseData("0", StateHistoryGenerator.Generate(), 0);
                yield return new TestCaseData("1", StateHistoryGenerator.Generate(), 1);
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
                yield return new TestCaseData("VisitCountWeight:1,1,1,2,3", StateHistoryGenerator.Generate(), 1);
                yield return new TestCaseData("VisitCountWeight:1,1,1,2,3", StateHistoryGenerator.Generate(1), 2);
                yield return new TestCaseData("VisitCountWeight:1,1,1,2,3", StateHistoryGenerator.Generate(1, 1), 3);
            }
        }

        [Test]
        [TestCaseSource(nameof(StaticWeightFromStringTestCases))]
        [TestCaseSource(nameof(VisitCountWeightFromStringTestCases))]
        [TestCaseSource(nameof(InvalidDescription))]
        public void ConnectionWeightParser_FromString_ReturnsExpectedWeight_GivenHistory(string input,
            List<State> stateHistory, int expectedWeight)
        {
            Assert.AreEqual(expectedWeight, ConnectionWeightParser.FromString(input).GetConnectionWeight(stateHistory));
        }
    }
}