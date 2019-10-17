using Aptacode.StateNet.Transitions;

namespace Aptacode.StateNet.StateTransitionTable
{
    public interface IStateTransitionTable
    {
        void Setup(StateCollection stateCollection, InputCollection inputCollection);

        void Set(Transition transition);

        Transition Get(string state, string input);

        void Clear(Transition transition);
    }
}