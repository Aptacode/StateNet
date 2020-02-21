using System;

namespace Aptacode.StateNet.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StartStateAttribute : Attribute
    {
        public StartStateAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}