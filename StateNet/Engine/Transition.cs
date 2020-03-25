using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine
{
    public class Transition
    {
        public Transition(State source, Input input, State target)
        {
            Source = source;
            Input = input;
            Target = target;
        }

        public State Source { get; set; }
        public Input Input { get; set; }
        public State Target { get; set; }
    }
}