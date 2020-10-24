using System;
using System.Linq.Expressions;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Network
{
    public class Connection
    {
        private readonly Func<int, int> _predicate;

        public Expression<Func<int, int>> Expression;

        public Connection(string target, string pattern, Expression<Func<int, int>> expression)
        {
            Target = target;
            Pattern = pattern;
            Expression = expression;
            _predicate = expression.Compile();
        }

        public Connection(string target, int staticWeight)
        {
            Target = target;
            Pattern = string.Empty;
            Expression = x => staticWeight;
            _predicate = Expression.Compile();
        }

        public string Target { get; }
        public string Pattern { get; }


        public int GetWeight(TransitionHistory entity)
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                return _predicate(0);
            }

            var matchCount = entity.MatchCount(Pattern);
            return _predicate(matchCount);
        }
    }
}