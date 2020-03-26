using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.Persistence
{
    public interface IStateNetworkSerializer
    {
        IStateNetwork Read();
        void Write(IStateNetwork stateNetwork);
    }
}