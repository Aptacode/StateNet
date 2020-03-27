using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine.Connections;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Connections;

namespace Aptacode.StateNet.Tests.Mocks
{
    public static class DummyConnections
    {
        public static List<Connection> Generate(string source = "defaultState", string input = "defaultInput",
            params int[] choices)
        {
            var output = new List<Connection>();
            for (var i = 0; i < choices.Length; i++)
            {
                output.Add(new Connection(new State(source), new Input(input), new State(i.ToString()),
                    new ConnectionWeight(choices[i])));
            }

            return output;
        }

        public static IEnumerable<Connection> Generate(params (string, string, string, int)[] choices)
        {
            return choices.Select(choice => Generate(choice.Item1, choice.Item2, choice.Item3, choice.Item4));
        }

        public static Connection Generate(string from, string input, string to, int weight)
        {
            return new Connection(new State(from), new Input(input), new State(to), new ConnectionWeight(weight));
        }

        public static ConnectionDistribution CreateDistribution(params (string, string, string, int)[] connections)
        {
            var connectionDistribution = new ConnectionDistribution();

            foreach (var connection in connections)
            {
                connectionDistribution.Add(
                    new Connection(new State(connection.Item1), new Input(connection.Item2),
                        new State(connection.Item3), new ConnectionWeight(connection.Item4)), connection.Item4);
            }

            return connectionDistribution;
        }
    }
}