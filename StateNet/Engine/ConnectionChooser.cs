using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine
{
    public class ConnectionChooser
    {
        private readonly EngineHistory _engineHistory;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        /// <summary>
        ///     Chooses a state from a given StateDistribution based on the past states stored in its StateHistory
        /// </summary>
        /// <param name="randomNumberGenerator"></param>
        /// <param name="engineHistory"></param>
        public ConnectionChooser(IRandomNumberGenerator randomNumberGenerator, EngineHistory engineHistory)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _engineHistory = engineHistory;
        }

        /// <summary>
        ///     Return the next state based on the given StateDistribution and the current StateHistory
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public Connection Choose(IEnumerable<Connection> connections)
        {
            var connectionWeights = GetConnectionWeights(connections).ToList();

            //If the total weight is 0 no state can be entered
            var totalWeight = SumWeights(connectionWeights);
            if (totalWeight == 0)
            {
                return null;
            }

            //Get a random number between 1 and the totalWeight + 1
            var choice = _randomNumberGenerator.Generate(1, totalWeight + 1);

            //Iterate over each connection weight keeping a running total of their sum in the weightCounter
            //Return the state where weightCounter >= choice
            using (var iterator = connectionWeights.GetEnumerator())
            {
                var weightCounter = 0;
                while (weightCounter < choice && iterator.MoveNext())
                {
                    weightCounter += iterator.Current.Item2;
                }

                return iterator.Current.Item1;
            }
        }

        /// <summary>
        ///     Calculates the weight of each connections in the StateDistribution given the current state history and returns a
        ///     list of State,Weight pairs
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        private IEnumerable<(Connection, int)> GetConnectionWeights(IEnumerable<Connection> connections)
        {
            return connections.Select(connection =>
                (connection, connection.ConnectionWeight.Evaluate(_engineHistory)));
        }

        /// <summary>
        ///     Calculates the sum of each connection in the given StateDistribution
        /// </summary>
        /// <param name="weights"></param>
        private static int SumWeights(IEnumerable<(Connection, int)> weights)
        {
            return weights
                .Sum(f => f.Item2 >= 0 ? f.Item2 : 0);
        }


        /// <summary>
        ///     Calculates the sum of each connections weight in the StateDistribution given the current StateHistory
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public int SumWeights(IEnumerable<Connection> connections) =>
            SumWeights(GetConnectionWeights(connections));
    }
}