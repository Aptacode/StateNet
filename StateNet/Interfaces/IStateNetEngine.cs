using System;
using Aptacode.StateNet.Engine.Events;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Interfaces
{
    public interface IStateNetEngine : IDisposable
    {
        State CurrentState { get; }
        IEngineHistory History { get; }
        bool Apply(string inputName);
        void Start();
        void Stop();

        #region Events

        void Subscribe(State state, Action callback);
        void Unsubscribe(State state, Action callback);

        event EventHandler<EngineFinishedEventArgs> OnFinished;

        event EventHandler<EngineStartedEventArgs> OnStarted;

        event EventHandler<EngineTransitionEventArgs> OnTransition;

        #endregion
    }
}