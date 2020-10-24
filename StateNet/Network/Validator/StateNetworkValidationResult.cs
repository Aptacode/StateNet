namespace Aptacode.StateNet.Network.Validator
{
    public class StateNetworkValidationResult
    {
        private StateNetworkValidationResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public string Message { get; }
        public bool Success { get; }

        public static StateNetworkValidationResult Fail(string message) =>
            new StateNetworkValidationResult(message, false);

        public static StateNetworkValidationResult Ok(string message) =>
            new StateNetworkValidationResult(message, true);
    }
}