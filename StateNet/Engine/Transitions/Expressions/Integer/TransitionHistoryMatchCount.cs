namespace Aptacode.StateNet.Engine.Transitions.Expressions.Integer
{
    public class TransitionHistoryMatchCount : BaseTransitionHistoryMatchCount
    {
        public TransitionHistoryMatchCount(params string?[] transitionPattern) : base(-1, transitionPattern) { }

        public override int Interpret(TransitionHistory context) =>
            context.GetTransitionHistory().MatchCount(TransitionPattern);
    }
}