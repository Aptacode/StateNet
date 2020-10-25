using Aptacode.StateNet.Engine.Transitions.Expressions.Integer;

namespace Aptacode.StateNet.Engine.Transitions.Expressions.Boolean
{
    public class Conditional : TernaryIntegerExpression
    {
        public override int Interpret(TransitionHistory context) => Condition.Interpret(context)
            ? PassExpression.Interpret(context)
            : FailExpression.Interpret(context);
    }
}