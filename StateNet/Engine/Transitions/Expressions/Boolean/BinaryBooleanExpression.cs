using Aptacode.StateNet.Engine.Transitions.Expressions.Integer;

namespace Aptacode.StateNet.Engine.Transitions.Expressions.Boolean
{
    public abstract class BinaryBooleanExpression : IBooleanExpression
    {
        public IIntegerExpression LHS { get; set; }

        public IIntegerExpression RHS { get; set; }

        public abstract bool Interpret(TransitionHistory context);
    }
}