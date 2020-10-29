using Aptacode.StateNet.Network;

namespace StateNet.Tests.Network.Helpers
{
    public static class StateNetwork_Helpers
    {
        public static StateNetwork Valid_StaticWeight_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Valid_StaticWeight_NetworkDictionary, "a");

        public static StateNetwork Invalid_StartState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Minimal_Valid_Connected_NetworkDictionary, "c"); //Takes a state dictionary with states connected 'a' <-> 'b', sets start state to 'c'

        public static StateNetwork Invalid_ConnectionTargetState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionTargetState_NetworkDictionary, "a");

        public static StateNetwork Invalid_ConnectionPatternState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionPatternState_NetworkDictionary, "a");

        public static StateNetwork Invalid_ConnectionPatternInput_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionPatternInput_NetworkDictionary, "a");
        public static StateNetwork SingleState_Network_StartState_NotSet =>
            new StateNetwork(StateNetworkDictionary_Helpers.SingleState_NetworkDictionary, "");
        public static StateNetwork Empty_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Empty_NetworkDictionary, "");

    }
}