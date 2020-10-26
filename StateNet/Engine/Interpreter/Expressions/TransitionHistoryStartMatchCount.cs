using System.Linq;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions
{
    public class TransitionHistoryStartMatchCount : BaseTransitionHistoryMatchCount
    {
        public TransitionHistoryStartMatchCount(int transitionCount, params string?[] transitionPattern) : base(
            transitionCount, transitionPattern) { }

        public override int Interpret(TransitionHistory context)
        {
            return context.GetMatches(TransitionPattern).Count(i => i <= TransitionCount);
        }
    }
}