using System;
using System.Collections.Generic;
using Aptacode.Expressions;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Validator;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class TransitionHistory : IContext
    {
        private readonly StateNetwork _network;
        private readonly Dictionary<int?[], MatchTracker> _patternMatches = new Dictionary<int?[], MatchTracker>();
        private readonly List<string> _stringTransitionHistory = new List<string>();
        private readonly List<int> _transitionHistory = new List<int>();

        public TransitionHistory(StateNetwork network)
        {
            _network = network;

            if (string.IsNullOrEmpty(network?.StartState))
            {
                throw new ArgumentNullException(nameof(network));
            }

            _transitionHistory.Add(network.StartState.GetHashCode());
            _stringTransitionHistory.Add(network.StartState);
            CreateMatchTrackers();
        }

        public int TransitionCount { get; private set; }

        private void CreateMatchTrackers()
        {
            foreach (var connection in _network.GetAllConnections())
            {
                var patterns = new HashSet<int?[]>();
                connection.Expression.GetPatterns(patterns);
                foreach (var pattern in patterns)
                {
                    var matchTracker = new MatchTracker(pattern);
                    matchTracker.Add(0, _network.StartState.GetHashCode());
                    _patternMatches.Add(pattern, matchTracker);
                }
            }
        }

        public IReadOnlyList<int> GetTransitionHistory() => _transitionHistory.AsReadOnly();

        public IEnumerable<int> GetMatches(int?[] pattern)
        {
            if (_patternMatches.TryGetValue(pattern, out var matchTracker))
            {
                return matchTracker.MatchTransitionIndexes;
            }

            return new int[0];
        }

        public void Add(string input, string destination)
        {
            var inputHashCode = input.GetHashCode();
            var destinationHashCode = destination.GetHashCode();
            _stringTransitionHistory.Add(input);
            _stringTransitionHistory.Add(destination);
            _transitionHistory.Add(inputHashCode);
            _transitionHistory.Add(destinationHashCode);

            TransitionCount++;

            foreach (var matchTracker in _patternMatches)
            {
                matchTracker.Value.Add(TransitionCount, inputHashCode);
                matchTracker.Value.Add(TransitionCount, destinationHashCode);
            }
        }

        public override string ToString() => string.Join(",", _stringTransitionHistory);
    }
}