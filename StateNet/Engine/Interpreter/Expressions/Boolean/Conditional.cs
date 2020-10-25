using Aptacode.StateNet.Engine.Interpreter.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Boolean
{
    public class Conditional : TernaryIntegerExpression
    {
        public override int Interpret(TransitionHistory context) => Condition.Interpret(context)
            ? PassExpression.Interpret(context)
            : FailExpression.Interpret(context);
    }
}