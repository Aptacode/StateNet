using System.Collections.Generic;

namespace Aptacode.StateNet.Interfaces
{
    public interface INodeWeight
    {
        int GetWeight(List<Node> history);
    }
}
