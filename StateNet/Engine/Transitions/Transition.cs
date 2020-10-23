namespace Aptacode.StateNet.Engine.Transitions
{
    public class Transition
    {
        public Transition(string source, string input, string destination)
        {
            Source = source;
            Input = input;
            Destination = destination;
        }

        public string Source { get; }
        public string Input { get; }
        public string Destination { get; }
    }
}