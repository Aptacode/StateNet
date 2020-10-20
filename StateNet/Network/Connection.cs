using System;
using System.Linq.Expressions;
using Aptacode.StateNet.Engine;

namespace Aptacode.StateNet.Network {
    public class Connection
    {
        public Connection(string target, Expression<Func<TransitionHistory, uint>> expression)
        {
            Target = target;
            Expression = expression;
            _predicate = expression.Compile();
        }

        public string Target { get; }

        public Expression<Func<TransitionHistory, uint>> Expression;

        private readonly Func<TransitionHistory, uint> _predicate;

        public uint GetWeight(TransitionHistory entity)
        {
            return _predicate(entity);
        }
    }
}