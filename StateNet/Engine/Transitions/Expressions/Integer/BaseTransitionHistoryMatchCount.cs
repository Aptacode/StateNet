using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public abstract class BaseTransitionHistoryMatchCount : IIntegerExpression
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