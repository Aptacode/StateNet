namespace Aptacode.StateNet.Engine {
    public class Transition
    {
        public string Source { get; }
        public string Input { get; }
        public string Destination { get; }

        public Transition(string source, string input, string destination)
        {
            Source = source;
            Input = input;
            Destination = destination;
        }
    }
}