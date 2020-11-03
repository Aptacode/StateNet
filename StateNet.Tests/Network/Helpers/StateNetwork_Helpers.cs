using Aptacode.StateNet.Network;

namespace StateNet.Tests.Network.Helpers
{
    public static class StateNetwork_Helpers
    {
        public static StateNetwork Minimal_Valid_Connected_StaticWeight_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkDictionary,
                "a");

        public static StateNetwork Invalid_StartState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Minimal_Valid_Connected_StaticWeight_NetworkDictionary,
                "c"); //Takes a state dictionary with states connected 'a' <-> 'b', sets start state to 'c'

        public static StateNetwork Invalid_ConnectionTargetState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionTargetState_NetworkDictionary, "a");

        public static StateNetwork Invalid_ConnectionPatternState_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionPatternState_NetworkDictionary, "a");

        public static StateNetwork Invalid_ConnectionPatternInput_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_ConnectionPatternInput_NetworkDictionary, "a");

        public static StateNetwork StartState_NotSet_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.SingleState_NetworkDictionary, "a") {StartState = ""};

        //To make this test even a thing I made StateNetwork.StartState settable, might not be the correct idea if the exception is thrown beforehand in the constructor anyway.
        public static StateNetwork Empty_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Empty_NetworkDictionary, "");

        public static StateNetwork Invalid_Unreachable_State_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_Unreachable_State_NetworkDictionary, "a");

        public static StateNetwork Invalid_UnusableInput_Network =>
            new StateNetwork(StateNetworkDictionary_Helpers.Invalid_UnusableInput_NetworkDictionary, "a");
    }
}