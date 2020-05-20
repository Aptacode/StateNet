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

<<<<<<< HEAD
        public override string ToString()
        {
            return $"Engine Started Event: Start State({StartState})";
        }
=======
        public override string ToString() => $"Engine Started Event: Start State({StartState.ToString()})";
>>>>>>> a5b2b31a57b874631e2362be2b387ce6a95baaa4
    }
}