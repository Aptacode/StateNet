using System.Linq;
using Aptacode.Expressions.Bool;
using Aptacode.Expressions.Bool.Comparison;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine;
using Aptacode.StateNet.Engine.Interpreter.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Random;
using Moq;
using Xunit;

namespace StateNet.Tests.Engine
{
    public class Engine_Tests
    {
        [Fact]
        public void EngineReverseTransition()
        {
            //Arrange
            var network = NetworkBuilder.New.SetStartState("A")
                .AddConnection("A", "Next", "B", new ConstantInteger<TransitionHistory>(1))
                .AddConnection("B", "Next", "A", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;
            
            //Act
            var sut = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var startState = sut.CurrentState;
            var secondState = sut.Apply("Next");
            var thirdState = sut.Apply("Next");

            //Assert
            Assert.Equal("A", startState);
            Assert.Equal("B", secondState.Transition.Destination);
            Assert.Equal("A", thirdState.Transition.Destination);
        }


        [Fact]
        public void EngineSimpleConnectionWeightSelection()
        {
            //Arrange
            var network = NetworkBuilder.New.SetStartState("A")
                .AddConnection("A", "Next", "B", new ConstantInteger<TransitionHistory>(1))
                .AddConnection("A", "Next", "C", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;

            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            mockRandomNumberGenerator
                .Setup(r => r.Generate(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);
            //Act
            var sut = new StateNetEngine(network, mockRandomNumberGenerator.Object);

            var startState = sut.CurrentState;
            var secondState = sut.Apply("Next");

            //Assert
            Assert.Equal("A", startState);
            Assert.Equal("C", secondState.Transition.Destination);
        }

        [Fact]
        public void EngineSingleTransition()
        {
            var network = NetworkBuilder.New.SetStartState("Start")
                .AddConnection("Start", "Next", "A", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;

            var sut = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var startState = sut.CurrentState;
            var secondState = sut.Apply("Next");

            Assert.Equal("Start", startState);
            Assert.Equal("A", secondState.Transition.Destination);
        }

        [Fact]
        public void EngineTransitionHistory()
        {
            var network = NetworkBuilder.New.SetStartState("A")
                .AddConnection("A", "Next", "B",
                    new Conditional<TransitionHistory>(
                        new LessThan<TransitionHistory>(
                            new TransitionHistoryMatchCount("B"),
                            new ConstantInteger<TransitionHistory>(1)),
                        new ConstantInteger<TransitionHistory>(1),
                        new ConstantInteger<TransitionHistory>(0)
                    ))
                .AddConnection("A", "Next", "C", new Conditional<TransitionHistory>(
                    new GreaterThanOrEqualTo<TransitionHistory>(
                        new TransitionHistoryMatchCount("B"),
                        new ConstantInteger<TransitionHistory>(1)),
                    new ConstantInteger<TransitionHistory>(1),
                    new ConstantInteger<TransitionHistory>(0)
                ))
                .AddConnection("B", "Next", "A", new ConstantInteger<TransitionHistory>(1))
                .AddConnection("C", "Next", "D", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;

            var sut = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var state1 = sut.CurrentState;
            var state2 = sut.Apply("Next");
            var state3 = sut.Apply("Next");
            var state4 = sut.Apply("Next");
            var state5 = sut.Apply("Next");

            Assert.Equal("A", state1);
            Assert.Equal("B", state2.Transition.Destination);
            Assert.Equal("A", state3.Transition.Destination);
            Assert.Equal("C", state4.Transition.Destination);
            Assert.Equal("D", state5.Transition.Destination);
        }

        [Fact]
        public void GetAvailableConnections_Returns_CorrectList_WhenConnectionsExistForCurrentStateAndInput()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start")
                .AddConnection("Start", "Next", "A", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;
            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            //Act
            var connections = sut.GetAvailableConnections("Next");
            //Assert
            Assert.Equal("A", connections.FirstOrDefault().Target);
        }

        [Fact]
        public void GetAvailableConnections_Returns_EmptyList_WhenNoConnectionsExistsForCurrentStateAndInput()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start")
                .Build().Network;
            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            //Act
            var connections = sut.GetAvailableConnections("Next");
            //Assert
            Assert.Empty(connections);
        }

        [Fact]
        public void GetAvailableInputs_Returns_CorrectList_WhenInputExistsForCurrentState()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start")
                .AddConnection("Start", "Next", "A", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;
            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            //Act
            var inputs = sut.GetAvailableInputs();
            //Assert
            Assert.Equal("Next", inputs.FirstOrDefault());
        }

        [Fact]
        public void GetAvailableInputs_Returns_EmptyList_WhenNoInputExistsForCurrentState()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start")
                .Build().Network;
            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            //Act
            var inputs = sut.GetAvailableInputs();
            //Assert
            Assert.Empty(inputs);
        }

        [Fact]
        public void InputNotDefined_ReturnsFailTransition()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("A")
                .AddConnection("A", "Next", "B", new ConstantInteger<TransitionHistory>(1))
                .AddConnection("A", "Next", "C", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());

            //Act
            var transitionResult = sut.Apply("Back");

            //Assert
            Assert.False(transitionResult.Success);
        }

        [Fact]

        public void CurrentStateChanges_After_SuccessfulTransition()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("A")
                .AddConnection("A", "Next", "B", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());

            //Act
            var successfulTransition = sut.Apply("Next");

            //Assert
            Assert.Equal("B", sut.CurrentState);
        }

        [Fact]

        public void CurrentStateDoesNotChange_After_FailedTransition()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("A")
                .AddConnection("A", "Next", "B", new ConstantInteger<TransitionHistory>(1)).Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());

            //Act
            var failedTransition = sut.Apply("Back");

            //Assert
            Assert.Equal("A", sut.CurrentState);
        }
         

        [Fact]
        public void OnTransition_Invoked_After_SuccessfulTransition()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start").AddConnection("Start", "Next", "A", new ConstantInteger<TransitionHistory>(1))
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            var OnTransitionWasCalled = false;
            sut.OnTransition += (_, __) => { OnTransitionWasCalled = true; };
            //Act
            var transitionResult = sut.Apply("Next");

            //Assert
            Assert.True(OnTransitionWasCalled);
        }

        [Fact]
        public void OnTransition_NotInvoked_After_FailedTransition()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start")
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            var OnTransitionWasCalled = false;
            sut.OnTransition += (_, __) => { OnTransitionWasCalled = true; };
            //Act
            var transitionResult = sut.Apply("Next");

            //Assert
            Assert.False(OnTransitionWasCalled);
        }
    }
}