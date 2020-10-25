namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public class TransitionHistoryEndMatchCount : BaseTransitionHistoryMatchCount
    {
        public TransitionHistoryEndMatchCount(int transitionCount, params string?[] transitionPattern) : base(
            transitionCount, transitionPattern) { }

        public override int Interpret(TransitionHistory context) => context.GetTransitionHistory()
            .TakeLast(TransitionElementCount).MatchCount(TransitionPattern);
    }
}