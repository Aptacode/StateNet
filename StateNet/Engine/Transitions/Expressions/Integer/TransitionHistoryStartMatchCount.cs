using System.Linq;

namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public class TransitionHistoryStartMatchCount : BaseTransitionHistoryMatchCount
    {
        public TransitionHistoryStartMatchCount(int transitionCount, params string?[] transitionPattern) : base(
            transitionCount, transitionPattern) { }

        public override int Interpret(TransitionHistory context) => context.GetTransitionHistory()
            .Take(TransitionElementCount).MatchCount(TransitionPattern);
    }
}