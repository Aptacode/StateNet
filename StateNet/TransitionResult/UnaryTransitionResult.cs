namespace Aptacode.StateNet.TransitionResult
{
    public class UnaryTransitionResult : TransitionResult
    {
        public UnaryTransitionResult(string message, bool success) : base(message, success)
        {

        }
        public UnaryTransitionResult(string message) : base(message, true)
        {

        }
    }
}
