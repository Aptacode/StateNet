using System.Linq;
using Aptacode.Expressions;
using Aptacode.StateNet.Engine;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.PatternMatching;
using Aptacode.StateNet.PatternMatching.Expressions;
using Aptacode.StateNet.Random;
using Moq;
using StateNet.Tests.Network.Helpers;
using Xunit;

namespace StateNet.Tests.Engine
{
    public class Engine_Tests
    {
        private readonly ExpressionFactory<TransitionHistory> _expressions = new ExpressionFactory<TransitionHistory>();

        [Fact]
        public void CurrentStateChanges_After_SuccessfulTransition()
        {
            //Arrange
            var networkResponse = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());

            //Act
            sut.Apply("1");

            //Assert
            Assert.Equal("b", sut.CurrentState);
        }

        [Fact]
        public void CurrentStateDoesNotChange_After_FailedTransition()
        {
            //Arrange
            var networkResponse = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());

            //Act
            sut.Apply("2");

            //Assert
            Assert.Equal("a", sut.CurrentState);
        }


        [Fact]
        public void Engine_Chooses_CorrectConnection_GivenWeights()
        {
            //Arrange
            var network = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .AddConnection("a", "1", "c", _expressions.Int(1))
                .Build().Network;

            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            mockRandomNumberGenerator
                .Setup(r => r.Generate(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);
            //Act
            var sut = new StateNetEngine(network, mockRandomNumberGenerator.Object);

            var startState = sut.CurrentState;
            var secondState = sut.Apply("1");

            //Assert
            Assert.Equal("a", startState);
            Assert.Equal("c", secondState.Transition.Destination);
        }

        [Fact]
        public void Engine_Chooses_CorrectConnection_GivenWeights_NetworkWithMultipleBranches()
        {
            //Arrange
            var network = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .AddConnection("a", "1", "c", _expressions.Int(1))
                .AddConnection("c", "1", "a", _expressions.Int(1))
                .AddConnection("c", "1", "b", _expressions.Int(1))
                .Build().Network;

            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            mockRandomNumberGenerator
                .Setup(r => r.Generate(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);
            //Act
            var sut = new StateNetEngine(network, mockRandomNumberGenerator.Object);

            var startState = sut.CurrentState;
            var secondState = sut.Apply("1");
            var thirdState = sut.Apply("1");

            //Assert
            Assert.Equal("a", startState);
            Assert.Equal("c", secondState.Transition.Destination);
            Assert.Equal("b", thirdState.Transition.Destination);
        }


        [Fact]
        public void EngineReverseTransition()
        {
            //Arrange
            var network = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .AddConnection("b", "1", "a", _expressions.Int(1))
                .Build().Network;

            //Act
            var sut = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var startState = sut.CurrentState;
            var secondState = sut.Apply("1");
            var thirdState = sut.Apply("1");

            //Assert
            Assert.Equal("a", startState);
            Assert.Equal("b", secondState.Transition.Destination);
            Assert.Equal("a", thirdState.Transition.Destination);
        }

        [Fact]
        public void EngineSingleTransition()
        {
            var network = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .Build()
                .Network;

            var sut = new StateNetEngine(network, new SystemRandomNumberGenerator());

            var startState = sut.CurrentState;
            var secondState = sut.Apply("1");

            Assert.Equal("a", startState);
            Assert.Equal("b", secondState.Transition.Destination);
        }

        [Fact]
        public void EngineTransitionHistory()
        {
            var network = NetworkBuilder.New.SetStartState("A")
                .AddConnection("A", "Next", "B",
                    _expressions.Conditional(
                        _expressions.LessThan(_expressions.Count(new Matches(new Pattern("B")))
                            ,
                            _expressions.Int(1)),
                        _expressions.Int(1),
                        _expressions.Int(0)))
                .AddConnection("A", "Next", "C",
                    _expressions.Conditional(
                        _expressions.GreaterThanOrEqualTo(
                            _expressions.Count(new Matches(new Pattern("B"))),
                            _expressions.Int(1)),
                        _expressions.Int(1),
                        _expressions.Int(0)))
                .AddConnection("B", "Next", "A", _expressions.Int(1))
                .AddConnection("C", "Next", "D", _expressions.Int(1))
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
            var networkResponse = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());

            //Act
            var transitionResult = sut.Apply("2");

            //Assert
            Assert.False(transitionResult.Success);
        }


        [Fact]
        public void OnTransition_Invoked_After_SuccessfulTransition()
        {
            //Arrange
            var networkResponse = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            var onTransitionWasCalled = false;
            sut.OnTransition += (_, __) => { onTransitionWasCalled = true; };
            //Act
            sut.Apply("1");

            //Assert
            Assert.True(onTransitionWasCalled);
        }

        [Fact]
        public void OnTransition_NotInvoked_After_FailedTransition()
        {
            //Arrange
            var networkResponse = NetworkBuilder.New
                .SetStartState("Start")
                .Build().Network;

            var sut = new StateNetEngine(networkResponse, new SystemRandomNumberGenerator());
            var onTransitionWasCalled = false;
            sut.OnTransition += (_, __) => { onTransitionWasCalled = true; };
            //Act
            sut.Apply("Next");

            //Assert
            Assert.False(onTransitionWasCalled);
        }
    }
}