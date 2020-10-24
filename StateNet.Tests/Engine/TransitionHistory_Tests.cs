using System;
using System.Collections.Generic;
using Aptacode.StateNet.Engine.Transitions;
using Xunit;

namespace StateNet.Tests.Engine
{
    public class TransitionHistory_Tests
    {
        [Theory]
        [ClassData(typeof(TransitionHistoryMatchCountTestData))]
        public void MatchCount_Returns_ExpectedMatchCount_ForTransitions(string startState,
            IEnumerable<(string input, string destination)> transitions, string pattern, int expectedMatchCount)
        {
            //Arrange
            var sut = new TransitionHistory(startState);
            foreach (var transition in transitions)
            {
                sut.Add(transition.input, transition.destination);
            }

            //Act
            var actualMatchCount = sut.MatchCount(pattern);

            //Assert
            Assert.Equal(expectedMatchCount, actualMatchCount);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_WhenStartStateIsNull()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Arrange
                //Act
                var sut = new TransitionHistory("");
            });
        }

        [Fact]
        public void ToString_Returns_CorrectHistory_WithMultipleTransitions()
        {
            //Arrange
            var sut = new TransitionHistory("a");
            sut.Add("1", "b");
            sut.Add("2", "c");
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("s<a>i<1>s<b>i<2>s<c>", actualResult);
        }

        [Fact]
        public void ToString_Returns_CorrectHistory_WithOneTransition()
        {
            //Arrange
            var sut = new TransitionHistory("A");
            sut.Add("Next", "B");
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("s<A>i<Next>s<B>", actualResult);
        }

        [Fact]
        public void ToString_Returns_StartState_WhenNoTransition()
        {
            //Arrange
            var sut = new TransitionHistory("A");
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("s<A>", actualResult);
        }
    }
}