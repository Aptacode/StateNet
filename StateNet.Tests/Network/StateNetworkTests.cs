using Aptacode.StateNet.Engine;
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
        public void GetConnections_Returns_EmptyList_WhenNoInputsAreDefined()
        {
            //Arrange
            var sut = new NetworkBuilder().SetStartState("Start").Build();

            //Act
            var connections = sut.GetConnections("Start", "Next");

            //Assert
            Assert.Empty(connections);
        }

        [Fact]
        public void GetConnections_Returns_List_WhenInputsAreDefined()
        {
            //Arrange
            var sut = new NetworkBuilder().SetStartState("Start").AddConnection("Start", "Next", "A", 1).Build();

            //Act
            var connections = sut.GetConnections("Start", "Next");

            //Assert
            Assert.Equal(1, connections.Count);
        }
    }
}
