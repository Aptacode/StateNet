using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.Events
{
    public class EngineStartedEventArgs : EngineEventArgs
    {
        public EngineStartedEventArgs(State startState)
        {
            StartState = startState;
        }

        public State StartState { get; set; }
    }
}