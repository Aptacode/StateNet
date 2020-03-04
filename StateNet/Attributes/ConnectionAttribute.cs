using System;

namespace Aptacode.StateNet.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ConnectionAttribute : Attribute
    {
        public ConnectionAttribute(string actionName, string targetName, string connectionDescription = "1")
        {
            TargetName = targetName;
            ActionName = actionName;
            ConnectionDescription = connectionDescription;
        }

        public string ActionName { get; set; }
        public string TargetName { get; set; }
        public string ConnectionDescription { get; set; }
    }
}