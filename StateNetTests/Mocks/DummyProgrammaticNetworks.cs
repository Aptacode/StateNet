using System.Collections.Generic;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.Mocks
{
    public static class DummyProgrammaticNetworks
    {
        public static IStateNetwork CreateNetwork(string startState, IEnumerable<State> states,
            IEnumerable<Input> inputs, IEnumerable<Connection> connections)
        {
            IStateNetwork stateNetwork = new StateNetwork();

            foreach (var state in states)
            {
                stateNetwork.CreateState(state);
            }

            foreach (var input in inputs)
            {
                stateNetwork.CreateInput(input);
            }

            foreach (var connection in connections)
            {
                stateNetwork.Connect(connection.Source, connection.Input, connection.Target,
                    connection.ConnectionWeight);
            }

            if (!string.IsNullOrEmpty(startState))
            {
                stateNetwork.StartState = stateNetwork.CreateState(startState);
            }

            return stateNetwork;
        }
    }
}