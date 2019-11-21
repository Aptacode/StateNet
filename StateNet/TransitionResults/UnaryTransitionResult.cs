namespace Aptacode.StateNet.TransitionResults
{
    public class UnaryTransitionResult : TransitionResult
    {
        /// <summary>
        /// Tells a UnaryTransition if the Transition was successful
        /// </summary>
        /// <param name="message"></param>
        /// <param name="success"></param>
        public UnaryTransitionResult(string message) : base(message, true) { }
        /// <summary>
        /// Tells a UnaryTransition if the Transition was successful
        /// </summary>
        /// <param name="message"></param>
        /// <param name="success"></param>
        public UnaryTransitionResult(string message, bool success) : base(message, success) { }
    }
}