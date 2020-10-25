using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Integer
{
    public class Subtract : BinaryIntegerExpression
    {
        public override int Interpret(TransitionHistory context) => LHS.Interpret(context) - RHS.Interpret(context);
    }
}