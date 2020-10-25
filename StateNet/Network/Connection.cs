using Aptacode.StateNet.Engine.Interpreter.Expressions.Integer;

namespace Aptacode.StateNet.Network
{
    public class Connection
    {
        public Connection(string target, IIntegerExpression expression)
        {
            Target = target;
            Expression = expression;
        }

        public IIntegerExpression Expression { get; }

        public string Target { get; }
    }
}