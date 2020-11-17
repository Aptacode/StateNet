using Aptacode.Expressions.Bool.Comparison;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;
using System.Collections.Generic;
using Aptacode.Expressions.Numeric;

namespace Aptacode.StateNet.Network
{
    public class Connection
    {
        public Connection(string target, INumericExpression<int, TransitionHistory> expression)
        {
            Target = target;
            Expression = expression;
        }

        public INumericExpression<int, TransitionHistory> Expression { get; }

        public string Target { get; }

    }
}