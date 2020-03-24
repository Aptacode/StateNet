using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Persistence
{
    public interface INetworkSerializer
    {
        StateNetwork Read();
        void Write(StateNetwork stateNetwork);
    }
}