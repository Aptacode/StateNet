using System.Linq;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions
{
    public class TransitionHistoryEndMatchCount : BaseTransitionHistoryMatchCount
    {
        public TransitionHistoryEndMatchCount(int transitionCount, params string?[] transitionPattern) : base(
            transitionCount, transitionPattern) { }

        public override int Interpret(TransitionHistory context)
        {
            var transitionIndex = context.TransitionCount - TransitionCount;
            return context.GetMatches(TransitionPattern).Count(i => i >= transitionIndex);
        }
    }
}