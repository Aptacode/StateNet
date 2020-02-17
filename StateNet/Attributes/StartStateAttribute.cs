using System;

namespace Aptacode.StateNet.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StartStateAttribute : Attribute
    {
        public string Name { get; set; }

        public StartStateAttribute(string name) => Name = name;
    }
}