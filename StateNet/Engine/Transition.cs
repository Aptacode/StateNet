using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine
{
    public class Transition
    {
        public Transition(State source, Input input, State destination)
        {
            Source = source;
            Input = input;
            Destination = destination;
        }

        public State Source { get; set; }
        public Input Input { get; set; }
        public State Destination { get; set; }
    }
}