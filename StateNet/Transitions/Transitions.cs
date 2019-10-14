namespace Aptacode.StateNet.Transitions
{
    public abstract class Transition
    {
        public string State { get; private set; }
        public string Input { get; private set; }
        public string Message { get; set; }

        protected Transition(string state, string input, string message)
        {
            State = state;
            Input = input;
            Message = message;
        }

        public abstract override string ToString();

        public abstract string Apply();
    }
}
