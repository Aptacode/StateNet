using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.Events
{
    public class EngineTransitionEventArgs : EngineEventArgs
    {
        public EngineTransitionEventArgs(State source, Input input, State target)
        {
            Transition = new Transition(source, input, target);
        }

        public EngineTransitionEventArgs(Transition transition)
        {
            Transition = transition;
        }

        public Transition Transition { get; set; }
    }
}