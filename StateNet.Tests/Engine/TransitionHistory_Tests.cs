using System;
using Aptacode.StateNet.Engine.Transitions;
using StateNet.Tests.Network;
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
                var sut = new TransitionHistory(null);
            });
        }

        [Fact]
        public void ToString_Returns_CorrectHistory_WithMultipleTransitions()
        {
            //Arrange
            var sut = new TransitionHistory(StateNetwork_Helpers.Valid_StaticWeight_Network);
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
            var sut = new TransitionHistory(StateNetwork_Helpers.Valid_StaticWeight_Network);
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
            var sut = new TransitionHistory(StateNetwork_Helpers.Valid_StaticWeight_Network);
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("a", actualResult);
        }
    }
}