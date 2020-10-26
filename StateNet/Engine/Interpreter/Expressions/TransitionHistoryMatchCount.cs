using System.Linq;
using Aptacode.StateNet.Engine.Transitions;

namespace Aptacode.StateNet.Engine.Interpreter.Expressions
{
    public class TransitionHistoryMatchCount : BaseTransitionHistoryMatchCount
    {
        public TransitionHistoryMatchCount(params string?[] transitionPattern) : base(-1, transitionPattern) { }


        public override int Interpret(TransitionHistory context) => context.GetMatches(TransitionPattern).Count();
    }
}