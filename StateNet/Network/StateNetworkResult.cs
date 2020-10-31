namespace Aptacode.StateNet.Network
{
    public class StateNetworkResult
    {
        private StateNetworkResult(string message, bool success, StateNetwork? network)
        {
            Message = message;
            Success = success;
            Network = network;
        }

        public string Message { get; }
        public bool Success { get; }
        public StateNetwork? Network { get; }
        public static StateNetworkResult Fail(StateNetwork? network, string message) => new StateNetworkResult(message, false, network);

        public static StateNetworkResult Ok(StateNetwork network, string message) =>
            new StateNetworkResult(message, true, network);
    }
}