namespace Aptacode.StateNet.Engine.Transitions.Expressions.Boolean
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