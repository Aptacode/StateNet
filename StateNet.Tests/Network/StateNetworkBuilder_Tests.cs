using Aptacode.StateNet.Network;
using Xunit;

namespace StateNet.Tests.Network
{
    public class StateNetworkBuilder_Tests
    {
        [Fact]
        public void Builder_Returns_FailedTransition_WhenStartStateIsNotSet()
        {
            //Arrange

            //Act
            var stateNetworkResult = NetworkBuilder.New.Build();

            //Assert
            Assert.False(stateNetworkResult.Success);
        }

        //public void Builder_Returns_FailedTransition_WhenNoConnectionsAreMadeSet()

        //StartState_IsSet_AfterBuild()
        //AddConnection_Creates_CorrectConnection()
    }
}