using System;
using System.Collections.Generic;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using Xunit;
using Moq;

namespace StateNet.Tests.Network
{
    public class StateNetwork_Tests
    {
        private IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>> _emptyDictionary;
        public StateNetwork_Tests()
        {
            _emptyDictionary = new Mock<IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>>().Object;
        }
        [Fact]
        public void Constructor_Throws_ArgumentNullException_WhenStartStateIsNull()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Arrange
                var nonemptyStateDictionary = new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>();
                nonemptyStateDictionary.Add("A", new Dictionary<string, IReadOnlyList<Connection>>());

                //Act
                var sut = new StateNetwork(
                    nonemptyStateDictionary, null);
            });
        }
        
        [Fact]
        public void Constructor_Throws_ArgumentNullException_WhenStartStateIsEmpty()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Arrange
                var nonemptyStateDictionary = new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>();
                nonemptyStateDictionary.Add("A", new Dictionary<string, IReadOnlyList<Connection>>());

                //Act
                var sut = new StateNetwork(
                    nonemptyStateDictionary, "");
            });
        }       
        
        [Fact]
        public void Constructor_Throws_ArgumentNullException_WhenStateDictionaryIsEmpty()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Arrange
                //Act
                var sut = new StateNetwork(
                    _emptyDictionary, "A");
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