using System;
using System.Collections;
using System.Collections.Generic;
using StateNet.Tests.Network.Helpers;

namespace StateNet.Tests.Network.Data
{
    public class StateNetwork_Constructor_TestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] {typeof(ArgumentNullException), null, "A"}, //Constructor throws ArgumentNullException when StateDictionary is null.
            new object[]
            {
                typeof(ArgumentException), StateNetworkDictionary_Helpers.Empty_NetworkDictionary, "A"
            }, //Constructor throws ArgumentException when StateDictionary is empty.
            new object[]
            {
                typeof(ArgumentNullException), StateNetworkDictionary_Helpers.SingleState_NetworkDictionary, ""
            }, //Constructor throws ArgumentNullException when the StartState is empty
            new object[]
            {
                typeof(ArgumentNullException), StateNetworkDictionary_Helpers.SingleState_NetworkDictionary, null
            } //Constructor throws ArgumentNullException when the StartState is null
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}