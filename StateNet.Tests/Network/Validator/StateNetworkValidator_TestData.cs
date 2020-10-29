using System.Collections;
using System.Collections.Generic;
using StateNet.Tests.Network.Helpers;

namespace StateNet.Tests.Network.Validator
{
    public class StateNetworkValidator_TestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {StateNetwork_Helpers.Invalid_ConnectionPatternInput_Network, "Connection had an invalid state or input dependency.", false },  //Determines StateNetwork is invali
            new object[] {StateNetwork_Helpers.Invalid_ConnectionPatternState_Network, "Connection had an invalid state or input dependency.", false },
            new object[] {StateNetwork_Helpers.Invalid_ConnectionTargetState_Network, "Connection target is not a valid state.", false },
            new object[] {StateNetwork_Helpers.Invalid_StartState_Network, "Start state was set to invalid state.", false },
            new object[] {StateNetwork_Helpers.Valid_StaticWeight_Network, "Success.", true}
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}