using Aptacode.Expressions;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Network
{
    public record Connection(string Target, IExpression<int, TransitionHistory> Expression);
}