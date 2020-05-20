using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.History
{
    public class Transition
    {
        public Transition(State source, Input input, State target)
        {
            Source = source;
            Input = input;
            Target = target;
        }

        public State Source { get; }
        public Input Input { get; }
        public State Target { get; }

<<<<<<< HEAD
        public override string ToString()
        {
            return $"Transition: Source({Source}) Input({Input}) Target({Target})";
        }
=======
        public override string ToString() => $"Transition: Source({Source.ToString()}) Input({Input.ToString()}) Target({Target.ToString()})";
>>>>>>> a5b2b31a57b874631e2362be2b387ce6a95baaa4
    }
}