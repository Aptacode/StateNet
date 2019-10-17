namespace Aptacode.StateNet.Transitions
{
    public abstract class Transition
    {
        protected Transition(string state, string input, string message)
        {
            State = state;
            Input = input;
            Message = message;
        }

        public string State { get; }
        public string Input { get; }
        public string Message { get; set; }

        public abstract override string ToString();

        public abstract string Apply();
    }
}