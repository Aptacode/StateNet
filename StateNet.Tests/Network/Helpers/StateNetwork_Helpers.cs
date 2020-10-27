using Aptacode.StateNet.Network;

namespace StateNet.Tests.Network
{
    public static class StateNetwork_Helpers
    {
     
        public static StateNetwork Valid_StaticWeight_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Valid_StaticWeight_NetworkDictionary, "a");

        public static StateNetwork Invalid_Detached_StartState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_Detached_StartState_Network_NetworkDictionary, "c");

        public static StateNetwork Invalid_ConnectionTargetState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionTargetState_NetworkDictionary, "a");

        public static StateNetwork Invalid_ConnectionPatternState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionPatternState_Network, "a");

        public static StateNetwork Invalid_ConnectionPatternInput_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionPatternInput_NetworkDictionary, "a");
    }
}