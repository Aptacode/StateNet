namespace Aptacode.StateNet.Engine.Transitions.Expressions.Boolean
{
    public class LessThanOrEqualTo : BinaryBooleanExpression
    {
        public override bool Interpret(TransitionHistory context) => LHS.Interpret(context) <= RHS.Interpret(context);
    }
}