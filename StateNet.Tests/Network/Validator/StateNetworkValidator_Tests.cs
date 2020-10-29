using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Validator;
using Xunit;

namespace StateNet.Tests.Network.Validator
{
    public class StateNetworkValidator_Tests
    {
        [Theory]
        [ClassData(typeof(StateNetworkValidator_TestData))]
        public void StateNetworkValidator_IsValidTests(StateNetwork stateNetwork, string message, bool isValid)
        {
            //Arrange
            //Act
            var stateNetworkValidationResult = stateNetwork.IsValid();

            //Assert
            Assert.Equal(isValid, stateNetworkValidationResult.Success);
            Assert.Equal(message, stateNetworkValidationResult.Message);
        }
    }
}