using System.Linq;
using Aptacode.Expressions;
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
        private readonly ExpressionFactory<TransitionHistory> _expressions = new ExpressionFactory<TransitionHistory>();


        [Fact]
        public void EngineReverseTransition()
        {
            var network = NetworkBuilder.New.SetStartState("A")
                .AddConnection("A", "Next", "B", _expressions.Int(1))
                .AddConnection("B", "Next", "A", _expressions.Int(1))
                .Build().Network;

            var engine = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var startState = engine.CurrentState;
            var secondState = engine.Apply("Next");
            var thirdState = engine.Apply("Next");

            Assert.Equal("A", startState);
            Assert.Equal("B", secondState.Transition.Destination);
            Assert.Equal("A", thirdState.Transition.Destination);
        }


        [Fact]
        public void EngineSimpleConnectionWeightSelection()
        {
            var network = NetworkBuilder.New.SetStartState("A")
                .AddConnection("A", "Next", "B", _expressions.Int(1))
                .AddConnection("A", "Next", "C", _expressions.Int(1))
                .Build().Network;

            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            mockRandomNumberGenerator
                .Setup(r => r.Generate(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            var engine = new StateNetEngine(network, mockRandomNumberGenerator.Object);

            var startState = engine.CurrentState;
            var secondState = engine.Apply("Next");

            Assert.Equal("A", startState);
            Assert.Equal("C", secondState.Transition.Destination);
        }

        [Fact]
        public void EngineSingleTransition()
        {
            var network = NetworkBuilder.New.SetStartState("Start")
                .AddConnection("Start", "Next", "A", _expressions.Int(1))
                .Build()
                .Network;

            var engine = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var startState = engine.CurrentState;
            var secondState = engine.Apply("Next");

            Assert.Equal("Start", startState);
            Assert.Equal("A", secondState.Transition.Destination);
        }

        [Fact]
        public void EngineTransitionHistory()
        {
            var network = NetworkBuilder.New.SetStartState("A")
                .AddConnection("A", "Next", "B",
                    _expressions.Conditional(
                        _expressions.LessThan(
                            new TransitionHistoryMatchCount("B"), 
                            _expressions.Int(1)), 
                        _expressions.Int(1), 
                        _expressions.Int(0)))

                .AddConnection("A", "Next", "C", 
                    _expressions.Conditional(
                    _expressions.GreaterThanOrEqualTo(
                        new TransitionHistoryMatchCount("B"), 
                        _expressions.Int(1)), 
                    _expressions.Int(1), 
                    _expressions.Int(0)))

                .AddConnection("B", "Next", "A", _expressions.Int(1))
                .AddConnection("C", "Next", "D", _expressions.Int(1))
                .Build().Network;

            var engine = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var state1 = engine.CurrentState;
            var state2 = engine.Apply("Next");
            var state3 = engine.Apply("Next");
            var state4 = engine.Apply("Next");
            var state5 = engine.Apply("Next");

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
                .SetStartState("Start").AddConnection("Start", "Next", "A", _expressions.Int(1))
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
                .SetStartState("Start").AddConnection("Start", "Next", "A", _expressions.Int(1))
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
                .AddConnection("A", "Next", "B", _expressions.Int(1))
                .AddConnection("A", "Next", "C", _expressions.Int(1))
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());

            //Act
            var transitionResult = sut.Apply("Back");

            //Assert
            Assert.False(transitionResult.Success);
        }

        //[Fact]

        //public void CurrentStateChanges_After_SuccessfulTransition()
        //{

        //}
        ////CurrentStateDoesNotChange_After_FailedTransition  

        [Fact]
        public void OnTransition_Invoked_After_SuccessfulTransition()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start").AddConnection("Start", "Next", "A", _expressions.Int(1))
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