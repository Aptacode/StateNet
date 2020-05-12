using System.Collections.Generic;
using Aptacode.StateNet.Network;
using System;
using Aptacode.StateNet.Engine.History;

namespace Aptacode.StateNet.Interfaces
{
    public interface IEngineHistory
    {
        void Log(State source, Input input, State target);

        int TransitionInCount(string input, string state);

        int TransitionOutCount(string state, string input);

        IEnumerable<Transition> GetLastTransitionsOut(string state, int count);
        IEnumerable<Transition> LastTransitions(int count);

        #region Inputs

        int InputAppliedCount(string name);
        IEnumerable<Input> Inputs { get; }

        #endregion

        #region States

        State StartState { get; }
        void SetStart(State source);
        int StateVisitCount(string name);

        IEnumerable<State> States { get; }
        IEnumerable<Transition> TransitionLog { get; }

        #endregion
    }
}