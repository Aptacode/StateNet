using System;

namespace Aptacode.StateNet.Interfaces
{
    public interface IEngine
    {
        void Subscribe(State state, Action callback);

        State Next(State state, string actionName);
    }
}