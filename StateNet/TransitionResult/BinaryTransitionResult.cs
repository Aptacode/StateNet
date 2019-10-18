namespace Aptacode.StateNet.TransitionResult
{
    public class BinaryTransitionResult : TransitionResult
    {
        /// <summary>
        ///     Tells a UnaryTransition if the Transition was successful and if so whether to move into the left or right state
        /// </summary>
        /// <param name="message"></param>
        /// <param name="success"></param>
        public BinaryTransitionResult(BinaryChoice choice, string message, bool success) : base(message, success)
        {
            Choice = choice;
        }

        /// <summary>
        ///     Tells a UnaryTransition if the Transition was successful and if so whether to move into the left or right state
        /// </summary>
        /// <param name="message"></param>
        /// <param name="success"></param>
        public BinaryTransitionResult(BinaryChoice choice, string message) : this(choice, message, true)
        {
        }

        public BinaryChoice Choice { get; set; }
    }
}