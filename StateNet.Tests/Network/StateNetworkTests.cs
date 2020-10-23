using Aptacode.StateNet.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StateNet.Tests.Network
{
    public class StateNetworkTests
    {
        [Fact]
        public void GetConnectionsReturnsEmptyListWhenNoneInputsAreDefined()
        {
            //Arrange
            var sut = new NetworkBuilder().SetStartState("Start").Build();

            //Act
            var connections = sut.GetConnections("Start", "Next");

            //Assert
            Assert.Empty(connections);
        }
    }
}
