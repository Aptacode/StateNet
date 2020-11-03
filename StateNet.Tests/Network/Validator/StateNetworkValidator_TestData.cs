using System.Collections;
using System.Collections.Generic;
using Aptacode.StateNet;
using StateNet.Tests.Network.Helpers;

namespace StateNet.Tests.Network.Validator
{
    public class StateNetworkValidator_TestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]
            {
                StateNetwork_Helpers.Invalid_ConnectionPatternInput_Network,
                Resources.INVALID_DEPENDENCY, false
            },
            new object[]
            {
                StateNetwork_Helpers.Invalid_ConnectionPatternState_Network,
                Resources.INVALID_DEPENDENCY, false
            },
            new object[]
            {
                StateNetwork_Helpers.Invalid_ConnectionTargetState_Network, Resources.INVALID_CONNECTION_TARGET,
                false
            },
            new object[]
                {StateNetwork_Helpers.Invalid_StartState_Network, Resources.INVALID_START_STATE, false},
            new object[] {StateNetwork_Helpers.StartState_NotSet_Network, Resources.UNSET_START_STATE, false},
            new object[] {StateNetwork_Helpers.Minimal_Valid_Connected_StaticWeight_Network, Resources.SUCCESS, true},
            new object[]
            {
                StateNetwork_Helpers.Invalid_Unreachable_State_Network, Resources.UNREACHABLE_STATES,
                false
            },
            new object[]
                {StateNetwork_Helpers.Invalid_UnusableInput_Network, Resources.UNUSABLE_INPUTS, false}
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}