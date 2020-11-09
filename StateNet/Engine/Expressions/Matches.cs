using System.Collections.Generic;
using System.Linq;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Expressions
{
    public class Matches : IListExpression<TransitionHistory>
    {
        public Matches(params string?[] pattern)
        {
            StringPattern = pattern;
            Pattern = pattern.Select(x => x?.GetHashCode()).ToArray();
        }

        public int?[] Pattern { get; }
        public IEnumerable<string?> StringPattern { get; }

        public int[] Interpret(TransitionHistory context) => context.GetMatches(Pattern).ToArray();
    }
}