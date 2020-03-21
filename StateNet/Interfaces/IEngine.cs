using System;
using System.Collections.Generic;
using Aptacode.StateNet.Events;

namespace Aptacode.StateNet.Interfaces
{
    public interface IEngine
    {
        State CurrentState { get; }
        event EngineEvent OnFinished;

        event EngineEvent OnStarted;

        event StateEvent OnTransition;
        List<(Input, State)> GetHistory();
        void Subscribe(State state, Action callback);
        void Unsubscribe(State state, Action callback);
        bool Apply(string inputName);
        void Start();
        void Stop();
    }
}