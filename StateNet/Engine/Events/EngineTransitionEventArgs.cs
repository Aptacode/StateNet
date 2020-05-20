using Aptacode.StateNet.Engine.History;
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

<<<<<<< HEAD
        public override string ToString()
        {
            return $"Engine Transition Event: {Transition}";
        }
=======
        public override string ToString() => $"Engine Transition Event: {Transition.ToString()}";
>>>>>>> a5b2b31a57b874631e2362be2b387ce6a95baaa4
    }
}