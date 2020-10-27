using System;
using System.Collections;
using System.Collections.Generic;

namespace StateNet.Tests.Network
{
    public class StateNetwork_Constructor_TestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] { typeof(ArgumentNullException), null, "A"},
            new object[] { typeof(ArgumentException), StateNetworkDictionary_Helpers.Invalid_NetworkDictionary_NoConnections, "A"},
            new object[] { typeof(ArgumentNullException), StateNetworkDictionary_Helpers.Valid_StaticWeight_NetworkDictionary, ""},
            new object[] { typeof(ArgumentNullException), StateNetworkDictionary_Helpers.Valid_StaticWeight_NetworkDictionary, null},
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}