using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;
using System.Linq;

namespace Aptacode.StateNet.Core.StateTransitionTable
{

    public interface IStateTransitionTable<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        void Set(Transition<States, Actions> transition);

        Transition<States, Actions> Get(States state, Actions action);

        void Clear(Transition<States, Actions> transition);
    }
}
