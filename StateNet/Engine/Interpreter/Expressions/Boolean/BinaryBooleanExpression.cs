using Aptacode.StateNet.Engine.Interpreter.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Boolean
{
    public abstract class BinaryBooleanExpression : IBooleanExpression
    {
        public IIntegerExpression LHS { get; set; }

        public IIntegerExpression RHS { get; set; }

        public abstract bool Interpret(TransitionHistory context);
    }
}