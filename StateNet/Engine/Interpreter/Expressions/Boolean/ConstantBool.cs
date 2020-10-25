using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Boolean
{
    public class ConstantBool : IBooleanExpression
    {
        public ConstantBool(bool value)
        {
            Value = value;
        }

        public bool Value { get; }

        public bool Interpret(TransitionHistory context) => true;
    }
}