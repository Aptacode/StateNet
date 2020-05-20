using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.Events
{
    public class EngineFinishedEventArgs : EngineEventArgs
    {
        public EngineFinishedEventArgs(State endState)
        {
            EndState = endState;
        }

        public State EndState { get; set; }
<<<<<<< HEAD

        public override string ToString()
        {
            return $"Engine Finished Event: End State({EndState})";
        }
=======
        public override string ToString() => $"Engine Finished Event: End State({EndState.ToString()})";
>>>>>>> a5b2b31a57b874631e2362be2b387ce6a95baaa4
    }
}