using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Integer
{
    public interface IIntegerExpression
    {
        int Interpret(TransitionHistory context);
    }
}