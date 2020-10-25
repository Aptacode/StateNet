using System;
using System.Collections.Generic;
using Aptacode.StateNet.Engine.Transitions.Expressions.Integer;
using Aptacode.StateNet.Network;
using Xunit;

namespace StateNet.Tests.Network
{
    public class StateNetwork_Tests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_WhenStartStateIsNull()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Arrange
                //Act
                var sut = new StateNetwork(
                    new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>(), "");
            });
        }

        [Fact]
        public void GetConnections_Returns_EmptyList_WhenNoInputsAreDefined()
        {
            //Arrange
            var sut = NetworkBuilder.New.SetStartState("Start").Build().Network;

            //Act
            var connections = sut.GetConnections("Start", "Next");

            //Assert
            Assert.Empty(connections);
        }

        [Fact]
        public void GetConnections_Returns_List_WhenInputsAreDefined()
        {
            //Arrange
            var sut = NetworkBuilder.New.SetStartState("Start")
                .AddConnection("Start", "Next", "A", new ConstantInteger(1)).Build().Network;

            //Act
            var connections = sut.GetConnections("Start", "Next");

            //Assert
            Assert.Equal(1, connections.Count);
        }

        //Constructor_Throws_ArgumentNullException_WhenStartStateIsEmpty
        //Constructor_Throws_ArgumentNullException_WhenStateDictionaryIsEmpty
        //Constructor_Throws_ArgumentNullException_WhenStateDictionaryNull

        //GetInputs_Returns_EmptyList_WhenStateDoesNotExist
        //GetInputs_Returns_List_WhenStateDoesExist

        //GetConnections_Returns_EmptyList_WhenNoConnectionsExistForStateAndInput
        //GetConnections_Returns_List_WhenConnectionsExistForStateAndInput
    }
}