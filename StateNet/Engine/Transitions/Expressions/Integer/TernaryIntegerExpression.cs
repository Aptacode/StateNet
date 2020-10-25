using Aptacode.StateNet.Engine.Transitions.Expressions.Boolean;

namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public abstract class TernaryIntegerExpression : IIntegerExpression
    {
        public IBooleanExpression Condition { get; set; }

        public IIntegerExpression PassExpression { get; set; }

        public IIntegerExpression FailExpression { get; set; }

        public abstract int Interpret(TransitionHistory context);
    }
}