using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class TransitionHistory
    {
        public static readonly string StateDelimiter = "s";
        public static readonly string InputDelimiter = "i";

        private readonly StringBuilder _historyStringBuilder = new StringBuilder();

        public TransitionHistory(string startState)
        {
            if (string.IsNullOrEmpty(startState))
            {
                throw new ArgumentNullException(nameof(startState));
            }

            _historyStringBuilder.Append($"{StateDelimiter}<{startState}>");
        }

        public void Add(string input, string destination)
        {
            _historyStringBuilder.Append($"{InputDelimiter}<{input}>{StateDelimiter}<{destination}>");
        }

        public override string ToString() => _historyStringBuilder.ToString();

        public int MatchCount(string pattern) => Regex.Matches(_historyStringBuilder.ToString(), pattern).Count;
    }
}