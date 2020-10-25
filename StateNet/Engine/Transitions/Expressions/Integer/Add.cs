namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public class Add : BinaryIntegerExpression
    {
        public override int Interpret(TransitionHistory context) => LHS.Interpret(context) + RHS.Interpret(context);
    }
}