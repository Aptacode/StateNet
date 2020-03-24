using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Persistence.JSon;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class SimpleDummyNetwork : StateNetwork
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

        [Test]
        public void CanSaveAndLoadNetworkFromFile()
        {
            var inputNetwork = new DummyNetwork();

            var serializer = new NetworkJsonSerializer("./test.json");
            serializer.Write(inputNetwork);
            var outputNetwork = serializer.Read();

            Assert.AreEqual(inputNetwork.ToString(), outputNetwork.ToString());
        }
    }
}