namespace Aptacode.StateNet.Engine.Transitions
{
    public record TransitionResult(string Message, bool Success, Transition? Transition)
    {
        public static TransitionResult Fail(string message) => new TransitionResult(message, false, null);

        public static TransitionResult Ok(Transition transition, string message) =>
            new TransitionResult(message, true, transition);
    }
}