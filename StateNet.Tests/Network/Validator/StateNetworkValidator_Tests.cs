using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Validator;
using Xunit;

namespace StateNet.Tests.Network.Validator
{
    public class StateNetworkValidator_Tests
    {
        [Theory]
        [ClassData(typeof(StateNetworkValidator_TestData))]
        public void Builder_Returns_FailedTransition_WhenStartStateIsNotSet(StateNetwork stateNetwork, bool isValid)
        {
            //Arrange
            //Act
            var stateNetworkResult = stateNetwork.IsValid();

            //Assert
            Assert.Equal(isValid, stateNetworkResult.Success);
        }
    }
}