using Aptacode.StateNet.Events;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Interfaces
{
    public interface IEngine
    {
        event EngineEvent OnFinished;

        event EngineEvent OnStarted;

        event StateEvent OnTransition;

        State CurrentState { get; }
        List<State> GetHistory();
        void Subscribe(State state, Action callback);
        void Unsubscribe(State state, Action callback);
        void Apply(string actionName);
        void Start();
        void Stop();
    }
}