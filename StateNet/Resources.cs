namespace Aptacode.StateNet
{
    public static class Resources
    {
        public static readonly string UNSET_START_STATE = "Start state was not set.";
        public static readonly string INVALID_START_STATE = "Start state was set to invalid state.";
        public static readonly string INVALID_CONNECTION_TARGET = "Connection target is not a valid state.";
        public static readonly string INVALID_DEPENDENCY = "Connection had an invalid state or input dependency.";
        public static readonly string UNREACHABLE_STATES = "Unreachable states exist in the network.";
        public static readonly string UNUSABLE_INPUTS = "Unusable inputs exist in the network.";
        public static readonly string SUCCESS = "Success.";
        public static readonly string NO_STATES = "No states were given.";

        public static string NO_AVAILABLE_CONNECTION(string currentState, string input) =>
            $"There are no available connections from {currentState} for {input}.";
    }
}