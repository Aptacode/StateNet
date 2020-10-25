using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Integer
{
    public class ConstantInteger : IIntegerExpression
    {
        public ConstantInteger(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public int Interpret(TransitionHistory context) => Value;
    }
}