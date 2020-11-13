using Aptacode.Expressions;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.Integer.List;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.PatternMatching.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aptacode.StateNet.PatternMatching.Expressions
{
    public class StateCountFromEnd : UnaryIntegerExpression<TransitionHistory>
    {
        public StateCountFromEnd(string state, int takeLast) : base(
            new Count<TransitionHistory>(
                new TakeLast<TransitionHistory>(
                    new Matches(
                        new Pattern(state)
                        ),
                    new ConstantInteger<TransitionHistory>(takeLast)
                    )
                )
            )
        { }

        public override int Interpret(TransitionHistory context)
        {
            return Expression.Interpret(context);
        }
    }
}
