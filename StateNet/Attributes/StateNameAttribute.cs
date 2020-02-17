using System;

namespace Aptacode.StateNet.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StateNameAttribute : Attribute
    {
        public string Name { get; set; }

        public StateNameAttribute(string name) => Name = name;
    }
}