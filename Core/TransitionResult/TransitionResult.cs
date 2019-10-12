namespace Aptacode.StateNet.Core.TransitionResult
{ 
    public abstract class TransitionResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        protected TransitionResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }
    }
}
