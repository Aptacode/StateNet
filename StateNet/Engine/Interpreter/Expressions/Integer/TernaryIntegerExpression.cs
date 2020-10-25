using Aptacode.StateNet.Engine.Interpreter.Expressions.Boolean;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Integer
{
    public abstract class TernaryIntegerExpression : IIntegerExpression
    {
        public IBooleanExpression Condition { get; set; }

        public IIntegerExpression PassExpression { get; set; }

        public IIntegerExpression FailExpression { get; set; }

        public abstract int Interpret(TransitionHistory context);
    }
}