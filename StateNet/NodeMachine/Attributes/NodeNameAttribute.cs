using System;
using System.Collections.Generic;
using System.Text;

namespace Aptacode.StateNet.NodeMachine.Attributes
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
