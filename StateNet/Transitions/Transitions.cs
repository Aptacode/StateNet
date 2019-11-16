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

        /// <summary>
        /// The starting state which the transition is relating to
        /// </summary>
        public string State { get; }

        /// <summary>
        /// The Input which when applied to the 'State' causes the transition
        /// </summary>
        public string Input { get; }

        /// <summary>
        /// A message describing the transition
        /// </summary>
        public string Message { get; set; }

        public abstract string Apply();

        public abstract override string ToString();
    }
}