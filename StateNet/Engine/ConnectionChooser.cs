using System.Collections.Generic;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine
{
    /// <summary>
    ///     Chooses a connection based on its evaluated weight
    /// </summary>
    public class ConnectionChooser
    {
        private readonly EngineHistory _engineHistory;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        /// <summary>
        ///     Evaluates the weights of a list of connections based on the EngineHistory
        ///     And then randomly chooses a connection influenced by the its weight
        /// </summary>
        /// <param name="randomNumberGenerator"></param>
        /// <param name="engineHistory"></param>
        public ConnectionChooser(IRandomNumberGenerator randomNumberGenerator, EngineHistory engineHistory)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _engineHistory = engineHistory;
        }

        /// <summary>
        ///     Evaluates the weights for a list of connections based on the EngineHistory
        ///     Chooses a connection randomly influenced by those weights
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public Connection Choose(IEnumerable<Connection> connections)
        {
            var connectionWeightDistribution = GetConnectionDistribution(connections);

            //If the total weight is 0 no state can be entered
            var totalWeight = connectionWeightDistribution.SumWeights();
            if (totalWeight == 0)
            {
                return null;
            }

            //Get a random number between 1 and the totalWeight + 1
            var choice = _randomNumberGenerator.Generate(1, totalWeight + 1);

            //Iterate over each connection weight keeping a running total of their sum in the weightCounter
            //Return the state where weightCounter >= choice
            using (var iterator = connectionWeightDistribution.GetEnumerator())
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
        ///     Calculates the weight of each connection given the current EngineHistory and returns a
        ///     list of State,Weight pairs
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        private ConnectionDistribution GetConnectionDistribution(IEnumerable<Connection> connections)
        {
            var connectionDistribution = new ConnectionDistribution();

            foreach (var connection in connections)
            {
                connectionDistribution.Add(connection, connection.ConnectionWeight.Evaluate(_engineHistory));
            }

            return connectionDistribution;
        }
    }
}