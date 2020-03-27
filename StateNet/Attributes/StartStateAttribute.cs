using System;

namespace Aptacode.StateNet.Attributes
{
    /// <summary>
    ///     Defines a state property to be the start state
    /// </summary>
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