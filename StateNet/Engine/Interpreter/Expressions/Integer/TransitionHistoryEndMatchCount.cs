using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions.Integer
{
    public class TransitionHistoryEndMatchCount : BaseTransitionHistoryMatchCount
    {
        public TransitionHistoryEndMatchCount(int transitionCount, params string?[] transitionPattern) : base(
            transitionCount, transitionPattern) { }

        public override int Interpret(TransitionHistory context) => context.GetTransitionHistory()
            .TakeLast(TransitionElementCount).MatchCount(TransitionPattern);
    }
}