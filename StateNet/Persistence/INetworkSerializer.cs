namespace Aptacode.StateNet.Persistence
{
    public interface INetworkSerializer
    {
        Network Read();
        void Write(Network network);
    }
}