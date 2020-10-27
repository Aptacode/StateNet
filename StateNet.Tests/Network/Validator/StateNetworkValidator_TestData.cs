using System.Collections;
using System.Collections.Generic;

namespace StateNet.Tests.Network.Validator
{
    public class StateNetworkValidator_TestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {StateNetwork_Helpers.Invalid_ConnectionPatternInput_Network, false},
            new object[] {StateNetwork_Helpers.Invalid_ConnectionPatternState_Network, false},
            new object[] {StateNetwork_Helpers.Invalid_ConnectionTargetState_Network, false},
            new object[] {StateNetwork_Helpers.Invalid_Detached_StartState_Network, false},
            new object[] {StateNetwork_Helpers.Valid_StaticWeight_Network, true}
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}