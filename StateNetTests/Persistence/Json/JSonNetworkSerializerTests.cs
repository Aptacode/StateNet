using Aptacode.StateNet.Network;
using Aptacode.StateNet.Tests.Mocks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Persistence.Json
{
    public class JSonNetworkSerializerTests
    {
        [Test]
        public void CanConvertNetworkToStringAndBack()
        {
            var inputNetwork = new DummyAttributeNetwork();


            var jsonString = JsonConvert.SerializeObject(inputNetwork);
            var outputNetwork = JsonConvert.DeserializeObject<StateNetwork>(jsonString);

            Assert.AreEqual(inputNetwork.ToString(), outputNetwork.ToString());
        }
    }
}