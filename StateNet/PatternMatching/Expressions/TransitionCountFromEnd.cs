using Aptacode.Expressions.GenericExpressions;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.List.Extensions;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.PatternMatching.Expressions
{
    public class TransitionCountFromEnd : UnaryExpression<int, TransitionHistory>
    {
        public TransitionCountFromEnd(string state, string input, int takeLast) : base(
            new Matches(new Pattern(state, input)).TakeLast(new ConstantInteger<TransitionHistory>(takeLast))
                .Count()) { }

        public override int Interpret(TransitionHistory context) => Expression.Interpret(context);
    }
}