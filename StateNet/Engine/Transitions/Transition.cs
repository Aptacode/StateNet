namespace Aptacode.StateNet.Engine.Transitions
{
    public class Transition
    {
        public readonly string Destination;
        public readonly string Input;

        public readonly string Source;

        public Transition(string source, string input, string destination)
        {
            Source = source;
            Input = input;
            Destination = destination;
        }
    }
}