using System.Collections.Generic;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.Mocks
{
    public static class DummyProgrammaticNetworks
    {
        public static IStateNetwork CreateNetwork(string startState, IEnumerable<string>states, IEnumerable<string> inputs, params (string, string, string, int)[] connections)
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
                stateNetwork.Connect(connection.Item1, connection.Item2, connection.Item3, new ConnectionWeight(connection.Item4));
            }

            if(!string.IsNullOrEmpty(startState))
                stateNetwork.SetStart(startState);

            return stateNetwork;
        }
    }
}