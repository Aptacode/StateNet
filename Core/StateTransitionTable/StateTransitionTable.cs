using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;
using System.Linq;

namespace Aptacode.StateNet.Core.StateTransitionTable
{
    public interface IStateTransitionTable
    {
        void Setup(StateCollection stateCollection, InputCollection inputCollection);

        void Set(Transition transition);

        Transition Get(string state, string input);

        void Clear(Transition transition);
    }
}
