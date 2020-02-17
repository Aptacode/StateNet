using System;

namespace Aptacode.StateNet.Interfaces
{
    public interface INodeEngine
    {
        void Subscribe(Node node, Action callback);

        Node Next(Node node, string actionName);
    }
}