using System.Linq;
using Aptacode.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using StateNet.Tests.Network.Helpers;
using Xunit;

namespace StateNet.Tests.Network
{
    public class StateNetworkBuilder_Tests
    {
        private static readonly ExpressionFactory<TransitionHistory> _expressions =
            new ExpressionFactory<TransitionHistory>();

        private readonly TransitionHistory _context;

        [Fact]
        public void
            AddConnection_SuccessfullyAddsConnection_AfterBuild() //This is dependent on some strange business with the context stuff to assert equality, not sure if this is perfect.
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.SingleState_NetworkBuilder;
            //Act
            var sut = networkBuilder.AddConnection("a", "1", "b", _expressions.Int(1)).Build();
            //Assert
            Assert.True(sut.Network.GetAllConnections().FirstOrDefault().Target ==
                        new Connection("b", _expressions.Int(1)).Target);
            Assert.True(_expressions
                .EqualTo(sut.Network.GetAllConnections().FirstOrDefault().Expression, _expressions.Int(1))
                .Interpret(_context));
            //
        }

        [Fact]
        public void Builder_Returns_FailMessage_WhenStartStateIsNotSet()
        {
            //Arrange
            //Act
            var sut = StateNetworkBuilder_Helpers.Empty_NetworkBuilder.Build();

            //Assert
            Assert.False(sut.Success);
        }

        [Fact]
        public void ClearConnectionsFromState_OfGivenInput_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrange
            var sut = StateNetworkBuilder_Helpers.Empty_NetworkBuilder;
            //Act
            sut.SetStartState("a");
            sut.AddConnection("a", "1", "b", _expressions.Int(1));
            sut.AddConnection("a", "2", "b", _expressions.Int(1));
            sut.ClearConnectionsFromState("a", "1");
            var result = sut.Build();

            Assert.Empty(result.Network.GetConnections("a", "1"));
            Assert.Equal(1, result.Network.GetConnections("a", "2").Count);
        }

        [Fact]
        public void ClearConnectionsFromState_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrange
            var sut = StateNetworkBuilder_Helpers.Empty_NetworkBuilder;
            //Act
            sut.SetStartState("a");
            sut.AddConnection("a", "1", "b", _expressions.Int(1));
            sut.AddConnection("a", "2", "b", _expressions.Int(1));
            sut.ClearConnectionsFromState("a");
            sut.AddConnection("a", "3", "b", _expressions.Int(1));
            var result = sut.Build();

            //Asset
            Assert.Empty(result.Network.GetConnections("a", "1"));
            Assert.Empty(result.Network.GetConnections("a", "2"));
            Assert.Equal(1, result.Network.GetConnections("a").Count);
            Assert.Equal(1, result.Network.GetConnections("a", "3").Count);
        }

        [Fact]
        public void ClearConnectionsToState_OfGivenInput_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrange
            var sut = StateNetworkBuilder_Helpers.Empty_NetworkBuilder;
            //Act
            sut.SetStartState("a");
            sut.AddConnection("a", "1", "b", _expressions.Int(1));
            sut.AddConnection("a", "2", "b", _expressions.Int(1));
            sut.ClearConnectionsToState("b", "1");
            sut.AddConnection("a", "3", "b", _expressions.Int(1));
            var result = sut.Build();

            //Asset
            Assert.Empty(result.Network.GetConnections("a", "1"));
            Assert.Equal(1, result.Network.GetConnections("a", "2").Count);
            Assert.Equal(1, result.Network.GetConnections("a", "3").Count);
            Assert.Equal(2, result.Network.GetConnections("a").Count);
        }

        [Fact]
        public void ClearConnectionsToState_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrange
            var sut = StateNetworkBuilder_Helpers.Empty_NetworkBuilder;
            //Act
            sut.SetStartState("a");
            sut.AddConnection("a", "1", "b", _expressions.Int(1));
            sut.AddConnection("a", "2", "b", _expressions.Int(1));
            sut.ClearConnectionsToState("b");
            sut.AddConnection("a", "3", "b", _expressions.Int(1));
            var result = sut.Build();

            //Asset
            Assert.Empty(result.Network.GetConnections("a", "1"));
            Assert.Empty(result.Network.GetConnections("a", "2"));
            Assert.Equal(1, result.Network.GetConnections("a").Count);
            Assert.Equal(1, result.Network.GetConnections("a", "3").Count);
        }

        [Fact]
        public void RemoveInput_SuccesfullyRemovesInput_AfterBuild()
        {
            //Arrange
            var sut = StateNetworkBuilder_Helpers.Empty_NetworkBuilder;
            //Act
            sut.SetStartState("a");
            sut.AddConnection("a", "1", "b", _expressions.Int(1));
            sut.AddConnection("a", "2", "b", _expressions.Int(1));
            sut.RemoveInputOnState("1", "a");
            var result = sut.Build();

            //Asset
            Assert.Empty(result.Network.GetConnections("a", "1"));
            Assert.Equal(1, result.Network.GetConnections("a").Count);
            Assert.Equal(1, result.Network.GetConnections("a", "2").Count);
        }

        [Fact]
        public void RemoveState_SuccesfullyRemovesState_AfterBuild()
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder;
            //Act
            var sut = networkBuilder.RemoveState("b").Build();

            //Assert
            Assert.DoesNotContain("b", sut.Network.GetAllStates());
        }

        [Fact]
        public void StartState_IsSet_AfterBuild()
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.Empty_NetworkBuilder;
            //Act
            var sut = networkBuilder.SetStartState("A").Build();

            //Assert
            Assert.Equal("A", sut.Network.StartState);
        }
    }
}