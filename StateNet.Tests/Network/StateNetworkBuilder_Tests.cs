using Aptacode.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using StateNet.Tests.Network.Helpers;
using System.Linq;
using Xunit;

namespace StateNet.Tests.Network
{
    public class StateNetworkBuilder_Tests
    {
        private static readonly ExpressionFactory<TransitionHistory> _expressions = new ExpressionFactory<TransitionHistory>();

        private readonly TransitionHistory _context;

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

        public void StartState_IsSet_AfterBuild()
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.Empty_NetworkBuilder;
            //Act
            var sut = networkBuilder.SetStartState("A").Build();

            //Assert
            Assert.Equal("A", sut.Network.StartState);
        }

        [Fact]

        public void AddState_SuccesfullyAddsState_AfterBuild()
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.SingleState_NetworkBuilder;
            //Act
            var sut = networkBuilder.AddState("b").Build();

            //Assert
            Assert.Contains("b", sut.Network.GetAllStates());
        }

        [Fact]

        public void AddInput_SuccesfullyAddsInput_AfterBuild()
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.SingleState_NetworkBuilder; //Can now add input without a defined connection, so this doesn't have to be a valid network
            //Act
            var sut = networkBuilder.AddInput("1").Build();

            //Assert
            Assert.Equal("Unusable inputs exist in the network.", sut.Message);
            Assert.Contains("1", sut.Network.GetAllInputs());

        }

        [Fact]
        public void AddConnection_SuccessfullyAddsConnection_AfterBuild() //This is dependent on some strange business with the context stuff to assert equality, not sure if this is perfect.
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.TwoState_Unconnected_NetworkBuilder;
            //Act
            var sut = networkBuilder.AddConnection("a", "1", "b", _expressions.Int(1)).Build();
            //Assert
            Assert.True(sut.Network.GetAllConnections().FirstOrDefault().Target == new Connection("b", _expressions.Int(1)).Target);
            Assert.True(_expressions.EqualTo(sut.Network.GetAllConnections().FirstOrDefault().Expression, _expressions.Int(1)).Interpret(_context));
            //
        }

        [Fact]
        public void ClearConnectionsFromState_OfGivenInput_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrance
            var networkBuilder = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder;
            //Act
            var sut = networkBuilder.ClearConnectionsFromState("a", "1").Build();
            //Assert
            Assert.Empty(sut.Network.GetAllConnections());
        }

        [Fact]
        public void ClearConnectionsFromState_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrance
            var networkBuilder = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder;
            //Act
            var sut = networkBuilder.ClearConnectionsFromState("a").Build();
            //Assert
            Assert.Empty(sut.Network.GetAllConnections());
        }

        [Fact]
        public void ClearConnectionsToState_OfGivenInput_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrance
            var networkBuilder = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder;
            //Act
            var sut = networkBuilder.ClearConnectionsToState("b", "1").Build();
            //Assert
            Assert.Empty(sut.Network.GetAllConnections());
        }

        [Fact]
        public void ClearConnectionsToState_SuccessfullyClearsConnection_AfterBuild()
        {
            //Arrance
            var networkBuilder = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder;
            //Act
            var sut = networkBuilder.ClearConnectionsToState("b").Build();
            //Assert
            Assert.Empty(sut.Network.GetAllConnections());
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

        public void RemoveInput_SuccesfullyRemovesInput_AfterBuild()
        {
            //Arrange
            var networkBuilder = StateNetworkBuilder_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkBuilder;
            //Act
            var sut = networkBuilder.AddConnection("b", "1", "c", _expressions.Int(1)).RemoveInputOnState("1", "a").Build();

            //Assert
            Assert.DoesNotContain("1", sut.Network.GetInputs("a"));
            Assert.Contains("1", sut.Network.GetInputs("b"));
        }
    }
}