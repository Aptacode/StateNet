using System.Collections.Generic;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Interfaces
{
    public interface IEngineHistory
    {
        void Log(State source, Input input, State target);

        int TransitionInCount(string input, string state);

        int TransitionOutCount(string state, string input);

        #region Inputs

        int InputAppliedCount(string name);
        IEnumerable<Input> Inputs { get; }

        #endregion

        #region States

        State StartState { get; }
        void SetStart(State source);
        int StateVisitCount(string name);

        IEnumerable<State> States { get; }

        #endregion
    }
}