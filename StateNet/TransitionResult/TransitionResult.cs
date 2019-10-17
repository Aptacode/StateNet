namespace Aptacode.StateNet.TransitionResult
{
    public abstract class TransitionResult
    {
        protected TransitionResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
    }
}