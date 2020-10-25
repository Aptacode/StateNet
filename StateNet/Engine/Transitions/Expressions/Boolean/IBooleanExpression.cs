namespace Aptacode.StateNet.Engine.Transitions.Expressions.Boolean
{
    public interface IBooleanExpression
    {
        bool Interpret(TransitionHistory context);
    }
}