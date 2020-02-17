using System.Collections.Generic;

namespace Aptacode.StateNet.Interfaces
{
    public interface IConnectionWeight
    {
        int GetWeight(List<State> history);
    }
}