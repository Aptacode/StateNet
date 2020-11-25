using Aptacode.Expressions.GenericExpressions;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.List.Extensions;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.PatternMatching.Expressions
{
    public class StateCountFromEnd : UnaryExpression<int, TransitionHistory>
    {
        public StateCountFromEnd(string state, int takeLast) : base(
            new Matches(new Pattern(state)).TakeLast(new ConstantInteger<TransitionHistory>(takeLast)).Count()) { }

        public override int Interpret(TransitionHistory context) => Expression.Interpret(context);
    }
}