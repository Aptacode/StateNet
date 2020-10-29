using System.Collections;
using System.Collections.Generic;
using StateNet.Tests.Network.Helpers;

namespace StateNet.Tests.Network.Validator
{
    public class StateNetworkValidator_TestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {StateNetwork_Helpers.Invalid_ConnectionPatternInput_Network, "Connection had an invalid state or input dependency.", false },
            new object[] {StateNetwork_Helpers.Invalid_ConnectionPatternState_Network, "Connection had an invalid state or input dependency.", false },
            new object[] {StateNetwork_Helpers.Invalid_ConnectionTargetState_Network, "Connection target is not a valid state.", false },
            new object[] {StateNetwork_Helpers.Invalid_StartState_Network, "Start state was set to invalid state.", false },
            new object[] {StateNetwork_Helpers.StartState_NotSet_Network, "Start state was not set.", false },
            new object[] {StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network, "Success.", true},
            new object[] {StateNetwork_Helpers.Invalid_Unreachable_State_Network, "Unreachable states exist in the network.", false},
            new object[] {StateNetwork_Helpers.Invalid_UnusableInput_Network, "Unusable inputs exist in the network.", false}


        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}