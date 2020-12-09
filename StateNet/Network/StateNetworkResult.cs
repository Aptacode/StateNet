namespace Aptacode.StateNet.Network
{
    public record StateNetworkResult(string Message, bool Success, StateNetwork? Network)
    {
        public static StateNetworkResult Fail(string message) => new StateNetworkResult(message, false, null);

        public static StateNetworkResult Ok(StateNetwork network, string message) =>
            new StateNetworkResult(message, true, network);
    }
}