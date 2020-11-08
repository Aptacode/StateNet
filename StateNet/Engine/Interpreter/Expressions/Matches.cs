using System.Collections.Generic;
using System.Linq;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions
{
    public class Matches : IListExpression<TransitionHistory>
    {
        public Matches(params string?[] transitionPattern)
        {
            TransitionPattern = transitionPattern.Select(x => x?.GetHashCode()).ToArray();
            TransitionStringPattern = transitionPattern;
        }

        public int?[] TransitionPattern { get; }
        public IEnumerable<string?> TransitionStringPattern { get; }

        public int[] Interpret(TransitionHistory context) => context.GetMatches(TransitionPattern).ToArray();
    }
}