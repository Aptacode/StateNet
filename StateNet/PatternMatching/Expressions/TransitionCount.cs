using Aptacode.Expressions;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.Integer.List;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.PatternMatching.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aptacode.StateNet.PatternMatching.Expressions
{
    public class TransitionCount : UnaryIntegerExpression<TransitionHistory>
    {
        public TransitionCount(string state, string input) : base(new Count<TransitionHistory>(new Matches(new Pattern(state, input)))) { }

        public override int Interpret(TransitionHistory context)
        {
            return Expression.Interpret(context);
        }
    }
}
