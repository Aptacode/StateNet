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

        public override string ToString()
        {
            return $"Engine Finished Event: End State({EndState})";
        }
    }
}