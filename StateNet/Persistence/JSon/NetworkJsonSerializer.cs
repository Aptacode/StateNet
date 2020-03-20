using System.IO;

namespace Aptacode.StateNet.Persistence.JSon
{
    public class NetworkJsonSerializer : INetworkSerializer
    {
        public NetworkJsonSerializer(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; set; }

        public Network Read()
        {
            Network network = null;

            using (var streamReader = new StreamReader(Filename))
            {
                network = NetworkToJSon.FromJSon(streamReader.ReadToEnd());
            }

            return network;
        }

        public void Write(Network network)
        {
            using (var streamWriter = new StreamWriter(Filename))
            {
                streamWriter.Write(NetworkToJSon.ToJson(network));
            }
        }
    }
}