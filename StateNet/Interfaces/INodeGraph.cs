using System.Collections.Generic;

namespace Aptacode.StateNet.Interfaces
{
    public interface INodeGraph
    {
        IEnumerable<Node> GetAll();

        IEnumerable<Node> GetEndNodes();

        bool IsValid();

        Node StartNode { get; set; }

        Node this[string nodeName] { get; }
        NodeChooser this[string nodeName, string action] { get; set; }
        NodeChooser this[Node node, string action] { get; set; }
    }

}
