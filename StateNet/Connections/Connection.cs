using Aptacode.StateNet.Connections.Weights;

namespace Aptacode.StateNet.Connections
{
    public sealed class Connection
    {
        public Connection(string fromState, string input, string toState, ConnectionWeight weight)
        {
            From = fromState;
            Input = input;
            To = toState;
            Weight = weight;
        }

        public string From { get; set; }
        public string Input { get; set; }
        public string To { get; set; }
        public ConnectionWeight Weight { get; set; }

        public override string ToString()
        {
            return $"{Input}({From})->{To}:{Weight}";
        }
    }
}