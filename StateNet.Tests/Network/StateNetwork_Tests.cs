using System;
using System.Collections.Generic;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using StateNet.Tests.Network.Data;
using Xunit;

namespace StateNet.Tests.Network
{
    public class StateNetwork_Tests
    {
        [Theory]
        [ClassData(typeof(StateNetwork_Constructor_TestData))]
        public void Constructor_Throws_Exception_Tests(Type exception,
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>> networkDictionary,
            string start) //Tests for each of the 4 cases in which an exception should be thrown by StateNetwork's constructor 
        {
            //Assert
            Assert.Throws(exception, () =>
            {
                //Act
                var sut = new StateNetwork(
                    networkDictionary, start);
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
                .AddConnection("Start", "Next", "A", new ConstantInteger<TransitionHistory>(1)).Build().Network;

            //Act
            var connections = sut.GetConnections("Start", "Next");

            //Assert
            Assert.Equal(1, connections.Count);
        }

        //
        //
        //Constructor_Throws_ArgumentNullException_WhenStateDictionaryNull

        //GetInputs_Returns_EmptyList_WhenStateDoesNotExist
        //GetInputs_Returns_List_WhenStateDoesExist

        //GetConnections_Returns_EmptyList_WhenNoConnectionsExistForStateAndInput
        //GetConnections_Returns_List_WhenConnectionsExistForStateAndInput
    }
}