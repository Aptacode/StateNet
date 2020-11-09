using System;
using System.Collections.Generic;
using Aptacode.Expressions;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class TransitionHistory : IContext
    {
        private readonly StateNetwork _network;
        private readonly Dictionary<int?[], PatternMatcher> _patternMatches = new Dictionary<int?[], PatternMatcher>();
        private readonly List<string> _stringTransitionHistory = new List<string>();
        private readonly List<int> _transitionHistory = new List<int>();

        public TransitionHistory(StateNetwork network)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));

            if (string.IsNullOrEmpty(network?.StartState))
            {
                throw new ArgumentNullException(nameof(network));
            }

            _transitionHistory.Add(_network.StartState.GetHashCode());
            _stringTransitionHistory.Add(_network.StartState);
            CreateMatchTrackers();
        }

        public int TransitionCount { get; private set; }

        private void CreateMatchTrackers()
        {
            foreach (var pattern in _network.Patterns)
            {
                var matchTracker = new PatternMatcher(pattern);
                matchTracker.Add(0, _network.StartState.GetHashCode());
                _patternMatches.Add(pattern, matchTracker);
            }
        }

        public IReadOnlyList<int> GetTransitionHistory() => _transitionHistory.AsReadOnly();

        public IEnumerable<int> GetMatches(int?[] pattern)
        {
            if (_patternMatches.TryGetValue(pattern, out var matchTracker))
            {
                return matchTracker.MatchList;
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