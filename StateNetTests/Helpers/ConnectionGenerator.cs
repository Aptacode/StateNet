using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.Helpers
{
    public static class ConnectionGenerator
    {
        public static List<Connection> Generate(string fromState = "defaultState", string input = "defaultInput",
            params int[] choices)
        {
            var output = new List<Connection>();
            for (var i = 0; i < choices.Length; i++)
            {
                output.Add(new Connection(new State(fromState), new Input(input), new State(i.ToString()),
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
    }
}