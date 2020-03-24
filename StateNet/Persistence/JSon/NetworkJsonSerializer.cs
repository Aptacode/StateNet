using System.IO;
using Aptacode.StateNet.Network;
using Newtonsoft.Json;

namespace Aptacode.StateNet.Persistence.JSon
{
    public class NetworkJsonSerializer : INetworkSerializer
    {
        public NetworkJsonSerializer(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; set; }

        public StateNetwork Read()
        {
            StateNetwork stateNetwork = null;

            using (var streamReader = new StreamReader(Filename))
            {
                stateNetwork = NetworkToJSon.FromJSon(streamReader.ReadToEnd());
            }

            return stateNetwork;
        }

        public void Write(StateNetwork stateNetwork)
        {
            using (var streamWriter = new StreamWriter(Filename))
            {
                streamWriter.Write(NetworkToJSon.ToJson(stateNetwork).ToString(Formatting.Indented));
            }
        }
    }
}