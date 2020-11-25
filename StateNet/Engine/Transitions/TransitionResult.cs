namespace Aptacode.StateNet.Engine.Transitions
{
    public class TransitionResult
    {
        public readonly string Message;
        public readonly bool Success;
        public readonly Transition? Transition;

        private TransitionResult(string message, bool success, Transition? transition)
        {
            Message = message;
            Success = success;
            Transition = transition;
        }

        public static TransitionResult Fail(string message) => new TransitionResult(message, false, null);

        public static TransitionResult Ok(Transition transition, string message) =>
            new TransitionResult(message, true, transition);
    }
}