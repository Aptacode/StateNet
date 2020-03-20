using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.Persistence.JSon;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class SimpleDummyNetwork : Network
    {
        [StartState("Start")]
        [Connection("Next", "End")]
        public State StartTestState { get; set; }

        [StateName("End")] public State EndTestState { get; set; }
    }

    public class JSonNetworkSerializerTests
    {
        [Test]
        public void CanConvertNetworkToStringAndBack()
        {
            var inputNetwork = new SimpleDummyNetwork();

            var jsonString = NetworkToJSon.ToJson(inputNetwork);
            var outputNetwork = NetworkToJSon.FromJSon(jsonString);

            Assert.AreEqual(inputNetwork.ToString(), outputNetwork.ToString());
        }
    }
}