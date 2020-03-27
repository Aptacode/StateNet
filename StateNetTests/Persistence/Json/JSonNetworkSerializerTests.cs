using Aptacode.StateNet.Persistence.Json;
using Aptacode.StateNet.Tests.Mocks;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Persistence.Json
{
    public class JSonNetworkSerializerTests
    {
        [Test]
        public void CanConvertNetworkToStringAndBack()
        {
            var inputNetwork = new DummyAttributeNetwork();

            var jsonString = StateNetworkJsonSerializer.ToJson(inputNetwork);
            var outputNetwork = StateNetworkJsonSerializer.FromJSon(jsonString);

            Assert.AreEqual(inputNetwork.ToString(), outputNetwork.ToString());
        }

        [Test]
        public void CanSaveAndLoadNetworkFromFile()
        {
            var inputNetwork = new DummyAttributeNetwork();

            var serializer = new StateNetworkJsonSerializer("./test.json");
            serializer.Write(inputNetwork);
            var outputNetwork = serializer.Read();

            Assert.AreEqual(inputNetwork.ToString(), outputNetwork.ToString());
        }
    }
}