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
    }
}