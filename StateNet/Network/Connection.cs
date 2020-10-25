using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Network
{
    public class Connection
    {
        public Connection(string target, IIntegerExpression<TransitionHistory> expression)
        {
            Target = target;
            Expression = expression;
        }

        public IIntegerExpression<TransitionHistory> Expression { get; }

        public string Target { get; }
    }
}