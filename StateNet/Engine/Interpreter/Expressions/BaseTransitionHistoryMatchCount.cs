using System.Collections.Generic;
using System.Linq;
using Aptacode.Expressions.Integer;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions
{
    public abstract class BaseTransitionHistoryMatchCount : IIntegerExpression<TransitionHistory>
    {
        protected BaseTransitionHistoryMatchCount(int transitionCount, params string?[] transitionPattern)
        {
            TransitionPattern = transitionPattern.Select(x => x?.GetHashCode());
            TransitionStringPattern = transitionPattern;
            TransitionCount = transitionCount;
            TransitionElementCount = transitionCount * 2;
        }

        public IEnumerable<int?> TransitionPattern { get; }
        public IEnumerable<string?> TransitionStringPattern { get; }
        public int TransitionCount { get; }
        public int TransitionElementCount { get; }

        public abstract int Interpret(TransitionHistory context);
    }
}