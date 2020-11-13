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
    public class StateCount : UnaryIntegerExpression<TransitionHistory>
    {
        public StateCount(string state) : base(
            new Count<TransitionHistory>(
                    new Matches(
                        new Pattern(state)
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
