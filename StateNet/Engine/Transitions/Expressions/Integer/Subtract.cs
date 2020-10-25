namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public class Subtract : BinaryIntegerExpression
    {
        public override int Interpret(TransitionHistory context) => LHS.Interpret(context) - RHS.Interpret(context);
    }
}