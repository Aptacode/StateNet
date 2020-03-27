using System;

namespace Aptacode.StateNet.Attributes
{
    /// <summary>
    ///     Defines a state property's name
    /// </summary>
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