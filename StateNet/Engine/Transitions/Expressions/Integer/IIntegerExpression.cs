namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public interface IIntegerExpression
    {
        int Interpret(TransitionHistory context);
    }
}