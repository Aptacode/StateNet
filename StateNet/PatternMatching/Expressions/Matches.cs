using System.Linq;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.PatternMatching.Expressions
{
    public class Matches : IListExpression<TransitionHistory>
    {
        public Matches(Pattern pattern)
        {
            Pattern = pattern;
        }

        public Pattern Pattern { get; }

        public int[] Interpret(TransitionHistory context) => context.GetMatches(Pattern).ToArray();
    }
}