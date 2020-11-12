using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.Expressions;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.PatternMatching;
using StateNet.Tests.Network.Data;
using StateNet.Tests.Network.Helpers;
using Xunit;

namespace StateNet.Tests.Network
{
    public class StateNetwork_Tests
    {
        private readonly ExpressionFactory<TransitionHistory> _expressions = new ExpressionFactory<TransitionHistory>();

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
                var sut = new StateNetwork(start,
                    networkDictionary, new Pattern[0]);
            });
        }

        [Fact]
        public void GetConnections_Returns_EmptyList_WhenInputIsNotDefinedForState()
        {
            //Arrange
            var sut = StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network;

            //Act
            var connections = sut.GetConnections("a", "2");

            //Assert
            Assert.Empty(connections);
        }


        [Fact]
        public void GetConnections_Returns_EmptyList_WhenNoConnectionsExistForStateAndInput()
        {
            //Arrange
            var sut = StateNetwork_Helpers.Invalid_UnusableInput_Network;

            //Act
            var connections = sut.GetConnections("b", "2");

            //Assert
            Assert.Empty(connections);
        }

        [Fact]
        public void GetConnections_Returns_List_WhenConnectionsExistForStateAndInput()
        {
            //Arrange
            var sut = StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network;
            //Act
            var connections = sut.GetConnections("a", "1");

            //Assert
            Assert.Equal(1, connections.Count);
        }

        [Fact]
        public void GetConnections_Returns_List_WhenInputsAreDefined()
        {
            //Arrange
            var sut = StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network;

            //Act
            var connections = sut.GetConnections("a", "1");

            //Assert
            Assert.Equal(1, connections.Count);
        }


        [Fact]
        public void GetInputs_Returns_EmptyList_WhenStateDoesNotExist()
        {
            //Arrange
            var network = StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network;

            //Act
            var sut = network.GetInputs("c");

            //Assert
            Assert.Empty(sut);
        }

        [Fact]
        public void GetInputs_Returns_List_WhenStateDoesExist()
        {
            //Arrange
            var network = StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network;
            //Act
            var sut = network.GetInputs("a");

            //Assert
            Assert.Contains("1", sut);
        }

        [Fact]

        public void GetAllInputs_Successfully_Returns_ListOfInputs()
        {
            //Arrange
            var network = StateNetwork_Helpers.State_WithMultiple_Inputs_Network;
            //Act
            var sut = network.GetAllInputs();
            //Assert
            Assert.Contains("1", sut);
            Assert.Contains("2", sut);
        }
    }
}