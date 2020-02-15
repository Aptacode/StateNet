using System;

namespace Aptacode.StateNet.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NodeStartAttribute : Attribute
    {
        public string Name;
        public NodeStartAttribute(string name)
        {
            Name = name;
        }
    }
}
