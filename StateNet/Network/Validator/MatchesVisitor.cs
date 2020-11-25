using System.Collections.Generic;
using System.Linq;
using Aptacode.Expressions.List;
using Aptacode.Expressions.Visitor;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.PatternMatching;
using Aptacode.StateNet.PatternMatching.Expressions;

namespace Aptacode.StateNet.Network.Validator
{
    public class MatchesVisitor : ExpressionVisitor<TransitionHistory>
    {
        private readonly HashSet<string?> _dependencies = new HashSet<string?>();
        private readonly HashSet<Pattern> _patterns = new HashSet<Pattern>();

        public IReadOnlyList<string?> Dependencies => _dependencies.ToList();
        public IReadOnlyList<Pattern> Patterns => _patterns.ToList();


        public override void Visit<TType>(TerminalListExpression<TType, TransitionHistory> expression)
        {
            if (expression is Matches matches)
            {
                _patterns.Add(matches.Pattern);

                foreach (var dependency in matches.Pattern.Elements)
                {
                    if (string.IsNullOrEmpty(dependency))
                    {
                        continue;
                    }

                    _dependencies.Add(dependency);
                }
            }

            base.Visit(expression);
        }
    }
}