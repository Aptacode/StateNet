namespace Aptacode.StateNet.Engine.Transitions.Expressions.Boolean
{
    public class GreaterThan : BinaryBooleanExpression
    {
        public override bool Interpret(TransitionHistory context) => LHS.Interpret(context) > RHS.Interpret(context);
    }
}