using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Boolean
{
    public interface IBooleanExpression
    {
        bool Interpret(TransitionHistory context);
    }
}