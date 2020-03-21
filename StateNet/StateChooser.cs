using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Connections;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{
    public class StateChooser
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly List<(Input, State)> _stateHistory;

        /// <summary>
        ///     Chooses a state from a given StateDistribution based on the past states stored in its StateHistory
        /// </summary>
        /// <param name="randomNumberGenerator"></param>
        /// <param name="stateHistory"></param>
        public StateChooser(IRandomNumberGenerator randomNumberGenerator, List<(Input, State)> stateHistory)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _stateHistory = stateHistory;
        }

        /// <summary>
        ///     Return the next state based on the given StateDistribution and the current StateHistory
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public string Choose(IEnumerable<Connection> connections)
        {
            var connectionWeights = GetConnectionWeights(connections).ToList();

            //If the total weight is 0 no state can be entered
            var totalWeight = TotalWeight(connectionWeights);
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
        private IEnumerable<(string, int)> GetConnectionWeights(IEnumerable<Connection> connections)
        {
            return connections.Select(connection =>
                (connection.To, connection.Weight.GetWeight(_stateHistory)));
        }

        /// <summary>
        ///     Calculates the sum of each connection in the given StateDistribution
        /// </summary>
        /// <param name="weights"></param>
        private int TotalWeight(IEnumerable<(string, int)> weights)
        {
            return weights
                .Sum(f => f.Item2 >= 0 ? f.Item2 : 0);
        }


        /// <summary>
        ///     Calculates the sum of each connections weight in the StateDistribution given the current StateHistory
        /// </summary>
        /// <param name="connections"></param>
        /// <returns></returns>
        public int TotalWeight(IEnumerable<Connection> connections)
        {
            return TotalWeight(GetConnectionWeights(connections));
        }
    }
}