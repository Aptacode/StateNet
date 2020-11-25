namespace Aptacode.StateNet.Network
{
    public class StateNetworkResult
    {
        public readonly string Message;
        public readonly StateNetwork? Network;
        public readonly bool Success;

        private StateNetworkResult(string message, bool success, StateNetwork? network)
        {
            Message = message;
            Success = success;
            Network = network;
        }

        public static StateNetworkResult Fail(string message) => new StateNetworkResult(message, false, null);

        public static StateNetworkResult Ok(StateNetwork network, string message) =>
            new StateNetworkResult(message, true, network);
    }
}