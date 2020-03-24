using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.Events
{
    public class EngineTransitionEventArgs : EngineEventArgs
    {
        public EngineTransitionEventArgs(State source, Input input, State destination)
        {
            Transition = new Transition(source, input, destination);
        }

        public EngineTransitionEventArgs(Transition transition)
        {
            Transition = transition;
        }

        public Transition Transition { get; set; }
    }
}