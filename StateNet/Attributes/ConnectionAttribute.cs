using System;

namespace Aptacode.StateNet.Attributes
{
    /// <summary>
    ///     The Connection Attribute represents a transition from the source state (the property for which the attribute is
    ///     defined)
    ///     to a target state under the given input
    ///     The connection has a weight defined in the Expression
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ConnectionAttribute : Attribute
    {
        public ConnectionAttribute(string input, string target) : this(input, target, "1")
        {
        }

        public ConnectionAttribute(string input, string target, string expression)
        {
            Target = target;
            Input = input;
            Expression = expression;
        }

        public string Input { get; set; }
        public string Target { get; set; }
        public string Expression { get; set; }
    }
}