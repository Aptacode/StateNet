using Aptacode.Expressions;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.PatternMatching.Expressions;
using System;
using System.Collections.Generic;
using System.Text;
using Aptacode.Expressions.GenericExpressions;
using Aptacode.Expressions.List.Extensions;
using Aptacode.Expressions.Numeric;
using Aptacode.Expressions.GenericExpressions;

namespace Aptacode.StateNet.PatternMatching.Expressions
{
    public class StateCount : UnaryExpression<int, TransitionHistory>
    {
        public StateCount(string state) : base(
                    new Matches(
                        new Pattern(state)
                        ).Count()
            )
        { }
        public override int Interpret(TransitionHistory context)
        {
            return Expression.Interpret(context);
        }
    }
}
