using Aptacode.Expressions;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Network
{
    public class Connection
    {
        public readonly IExpression<int, TransitionHistory> Expression;

        public readonly string Target;

        public Connection(string target, IExpression<int, TransitionHistory> expression)
        {
            Target = target;
            Expression = expression;
        }
    }
}