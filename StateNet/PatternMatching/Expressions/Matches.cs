using System.Linq;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.PatternMatching.Expressions
{
    public class Matches : TerminalListExpression<int, TransitionHistory>
    {
        public readonly Pattern Pattern;

        public Matches(Pattern pattern)
        {
            Pattern = pattern;
        }

        public override int[] Interpret(TransitionHistory context) => context.GetMatches(Pattern).ToArray();
    }
}