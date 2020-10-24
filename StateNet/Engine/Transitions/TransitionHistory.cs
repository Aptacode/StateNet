using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class TransitionHistory
    {
        private readonly StringBuilder _historyStringBuilder = new StringBuilder();

        public TransitionHistory(string startState)
        {
            if (string.IsNullOrEmpty(startState))
            {
                throw new ArgumentNullException(nameof(startState));
            }
            _historyStringBuilder.Append($"{startState}");
        }

        public void Add(string input, string destination)
        {
            _historyStringBuilder.Append($":{input}:{destination}");
        }

        public override string ToString() => _historyStringBuilder.ToString();

        public int MatchCount(string pattern) => Regex.Matches(_historyStringBuilder.ToString(), pattern).Count;
    }
}