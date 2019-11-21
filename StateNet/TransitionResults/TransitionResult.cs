namespace Aptacode.StateNet.TransitionResults
{
    public abstract class TransitionResult
    {
        protected TransitionResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        /// <summary>
        /// A message describing the result of the transition
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// True if the transition could be applied
        /// </summary>
        public bool Success { get; set; }
    }
}