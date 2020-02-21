using System;

namespace Aptacode.StateNet.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StateNameAttribute : Attribute
    {
        public StateNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}