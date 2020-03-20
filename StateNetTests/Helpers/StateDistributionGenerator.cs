using System.Collections.Generic;
using Aptacode.StateNet.Connections;
using Aptacode.StateNet.Connections.Weights;

namespace Aptacode.StateNet.Tests.Helpers
{
    public static class StateDistributionGenerator
    {
        public static List<Connection> Generate(params int[] nodeWeights)
        {
            var output = new List<Connection>();

            for (var i = 0; i < nodeWeights.Length; i++)
            {
                output.Add(new Connection("start", "action", i.ToString(), new StaticWeight(nodeWeights[i])));
            }

            return output;
        }
    }
}