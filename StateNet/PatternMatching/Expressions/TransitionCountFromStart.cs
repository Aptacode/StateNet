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
    public class TransitionCountFromStart : UnaryIntegerExpression<TransitionHistory>
    {
        public TransitionCountFromStart(string state, string input, int takeFirst) : base(
                        new Count<TransitionHistory>(
                new TakeFirst<TransitionHistory>(
                    new Matches(
                        new Pattern(state, input)
                        ),
                    new ConstantInteger<TransitionHistory>(takeFirst)
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
