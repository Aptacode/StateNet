using System.Collections.Generic;

namespace Aptacode.StateNet.Interfaces
{
    public interface IConnectionWeight
    {
        int GetConnectionWeight(List<State> stateHistory);
    }
}