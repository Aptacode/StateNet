using System.Collections.Generic;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Connections;

namespace Aptacode.StateNet.Tests.Mocks
{
    public static class DummyProgrammaticNetworks
    {
        /// <summary>
        ///     Creates a network with the provided list of states, inputs and connections
        /// </summary>
        /// <param name="startState"></param>
        /// <param name="states"></param>
        /// <param name="inputs"></param>
        /// <param name="connections"></param>
        /// <returns></returns>
        public static IStateNetwork Create(string startState, IEnumerable<State> states,
            IEnumerable<Input> inputs, IEnumerable<Connection> connections)
        {
            IStateNetwork stateNetwork = new StateNetwork();

            foreach (var state in states) stateNetwork.CreateState(state);

            foreach (var input in inputs) stateNetwork.CreateInput(input);

            foreach (var connection in connections)
                stateNetwork.Connect(connection.Source, connection.Input, connection.Target,
                    connection.ConnectionWeight);

            if (!string.IsNullOrEmpty(startState)) stateNetwork.StartState = stateNetwork.CreateState(startState);

            return stateNetwork;
        }
    }
}