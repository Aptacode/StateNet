using System;
using System.Linq.Expressions;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Network
{
    public class Connection
    {
        private readonly Func<TransitionHistory, int> _predicate;

        public Expression<Func<TransitionHistory, int>> Expression;

        public Connection(string target, Expression<Func<TransitionHistory, int>> expression)
        {
            Target = target;
            Expression = expression;
            _predicate = expression.Compile();
        }

        public string Target { get; }

        public int GetWeight(TransitionHistory entity) => _predicate(entity);
    }
}