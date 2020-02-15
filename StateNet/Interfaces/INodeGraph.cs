using System.Collections.Generic;

namespace Aptacode.StateNet.Interfaces
{
    public interface INodeGraph
    {
        IEnumerable<Node> GetAll();

        IEnumerable<Node> GetEndNodes();

        bool IsValid();

        Node StartNode { get; set; }
        Node Next(Node node, string actionName);
    }

}
