using System;
using System.Linq;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.PatternMatching;
using StateNet.Tests.Network.Helpers;
using Xunit;

namespace StateNet.Tests.Engine
{
    public class TransitionHistory_Tests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_WhenStartStateIsNull()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Arrange
                //Act
                new TransitionHistory(null);
            });
        }

        [Fact]
        public void GetMatches_Returns_CorrectMatches_SingleMatch()
        {
            //Arrange
            var sut = new TransitionHistory(StateNetwork_Helpers
                .Minimal_Valid_Connected_StaticWeight_Network_WithPattern);
            sut.Add("1", "b");
            //Act
            var pattern = new Pattern(StateNetwork_Helpers.StateB);
            var matches = sut.GetMatches(pattern);
            //Assert
            Assert.Equal("1", matches.First().ToString());
        }

        //GetMatchesTest
        //AddTest
        [Fact]
        public void ToString_Returns_CorrectHistory_WithMultipleTransitions()
        {
            //Arrange
            var sut = new TransitionHistory(StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network);
            sut.Add("1", "b");
            sut.Add("2", "c");
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("a,1,b,2,c", actualResult);
        }

        [Fact]
        public void ToString_Returns_CorrectHistory_WithOneTransition()
        {
            //Arrange
            var sut = new TransitionHistory(StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network);
            sut.Add("next", "b");
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("a,next,b", actualResult);
        }

        [Fact]
        public void ToString_Returns_StartState_WhenNoTransition()
        {
            //Arrange
            var sut = new TransitionHistory(StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network);
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("a", actualResult);
        }
    }
}