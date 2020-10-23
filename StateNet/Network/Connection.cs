using System;
using System.Linq.Expressions;
using Aptacode.StateNet.Engine;

namespace Aptacode.StateNet.Network {
    public class Connection
    {
        public Connection(string target, Expression<Func<TransitionHistory, int>> expression)
        {
            Target = target;
            Expression = expression;
            _predicate = expression.Compile();
        }

        public string Target { get; }

        public Expression<Func<TransitionHistory, int>> Expression;

        private readonly Func<TransitionHistory, int> _predicate;

        public int GetWeight(TransitionHistory entity)
        {
            return _predicate(entity);
        }
    }
}