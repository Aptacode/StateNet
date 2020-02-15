using System;

namespace Aptacode.StateNet.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NodeNameAttribute : Attribute
    {
        public string Name;
        public NodeNameAttribute(string name)
        {
            Name = name;
        }
    }
}
