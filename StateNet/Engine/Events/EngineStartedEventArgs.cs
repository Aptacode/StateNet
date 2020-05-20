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

        public override string ToString() => $"Engine Started Event: Start State({StartState.ToString()})"; }
}