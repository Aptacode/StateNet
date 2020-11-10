using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Aptacode.Expressions.List;
using Aptacode.Expressions.Visitor;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.PatternMatching.Expressions;

namespace Aptacode.StateNet.Network.Validator
{
    public class PatternVisitor : ExpressionVisitor<TransitionHistory>
    {
        private readonly HashSet<string?> _dependencies = new HashSet<string?>();

        public override void Visit(TerminalListExpression<TransitionHistory> expression)
        {
            if (expression is Matches matches)
            {
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

        public IReadOnlyList<string?> Dependencies => _dependencies.ToList();
    }
}
