using Aptacode.StateNet.Engine.Transitions;
using Xunit;

namespace StateNet.Tests.Engine
{
    public class TransitionHistory_Tests
    {
        [Fact]
        public void ToString_Returns_StartState_WhenNoTransition()
        {
            //Arrange
            var sut = new TransitionHistory("A");
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("A", actualResult);
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
            Assert.Equal("A:Next:B", actualResult);
        }

        [Fact]
        public void ToString_Returns_CorrectHistory_WithMultipleTransitions()
        {
            //Arrange
            var sut = new TransitionHistory("A");
            sut.Add("Next", "B");
            sut.Add("Next", "C");
            //Act
            var actualResult = sut.ToString();

            //Assert
            Assert.Equal("A:Next:B:Next:C", actualResult);
        }
    }
}
