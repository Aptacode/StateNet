namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public abstract class BinaryIntegerExpression : IIntegerExpression
    {
        public IIntegerExpression LHS { get; set; }

        public IIntegerExpression RHS { get; set; }

        public abstract int Interpret(TransitionHistory context);
    }
}