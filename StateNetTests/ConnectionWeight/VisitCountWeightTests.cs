using System.Collections.Generic;
using Aptacode.StateNet.ConnectionWeight;
using Aptacode.StateNet.Tests.Helpers;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.ConnectionWeight
{
    public class VisitCountWeightTests
    {
        public static IEnumerable<TestCaseData> ChangingHistoryTestCases
        {
            get
            {
                yield return new TestCaseData(1, 1, 10, 20, 30, null, 10, "Null history returns LessThen weight");

                yield return new TestCaseData(1, 1, 10, 20, 30, StateHistoryGenerator.Generate(), 10,
                    "0 visits less than comparison return 10");
                yield return new TestCaseData(1, 1, 10, 20, 30, StateHistoryGenerator.Generate(1), 20,
                    "1 visits equals comparison return 20");
                yield return new TestCaseData(1, 1, 10, 20, 30, StateHistoryGenerator.Generate(1, 1), 30,
                    "2 visits greater then comparison return 30");

                yield return new TestCaseData(1, 1, 10, 20, 30, StateHistoryGenerator.Generate(2), 10,
                    "0 visits less than comparison return 10");
                yield return new TestCaseData(1, 2, 10, 20, 30, StateHistoryGenerator.Generate(2, 1), 10,
                    "1 visits less than comparison return 10");
                yield return new TestCaseData(1, 2, 10, 20, 30, StateHistoryGenerator.Generate(1, 2, 1), 20,
                    "2 visits equals comparison return 20");
                yield return new TestCaseData(1, 2, 10, 20, 30, StateHistoryGenerator.Generate(1, 2, 1, 2, 1), 30,
                    "3 visits greater then comparison return 30");

                yield return new TestCaseData(1, 1, -1, -1, -1, StateHistoryGenerator.Generate(), 0,
                    "Should round negative weight to 0");
                yield return new TestCaseData(1, 1, -1, -1, -1, StateHistoryGenerator.Generate(1), 0,
                    "Should round negative weight to 0");
                yield return new TestCaseData(1, 1, -1, -1, -1, StateHistoryGenerator.Generate(1, 1), 0,
                    "Should round negative weight to 0");
            }
        }

        [Test]
        [TestCaseSource(nameof(ChangingHistoryTestCases))]
        public void GetWeight_Returns_ExpectedWeight(int state, int comparisonCount, int lessThenWeight,
            int equalToWeight, int greaterThenWeight, List<State> history, int expectedWeight, string message = "")
        {
            Assert.AreEqual(expectedWeight,
                new VisitCountWeight(state.ToString(), comparisonCount, lessThenWeight, equalToWeight,
                    greaterThenWeight).GetConnectionWeight(history), message);
        }
    }
}