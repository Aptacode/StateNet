using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class TransitionHistory
    {
        private readonly List<string> _stringTransitionHistory = new List<string>();
        private readonly List<int> _transitionHistory = new List<int>();

        public TransitionHistory(string startState)
        {
            if (string.IsNullOrEmpty(startState))
            {
                throw new ArgumentNullException(nameof(startState));
            }

            _transitionHistory.Add(startState.GetHashCode());
            _stringTransitionHistory.Add(startState);
        }

        public IReadOnlyList<int> GetTransitionHistory() => _transitionHistory.AsReadOnly();

        public void Add(string input, string destination)
        {
            _stringTransitionHistory.Add(input);
            _stringTransitionHistory.Add(destination);

            _transitionHistory.Add(input.GetHashCode());
            _transitionHistory.Add(destination.GetHashCode());
        }

        public override string ToString() => string.Join(",", _stringTransitionHistory);
    }
}