using System;

namespace Aptacode.StateNet.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class NodeConnectionAttribute : Attribute
    {
        public string ActionName { get; set; }
        public string TargetName { get; set; }
        public string ConnectionDescription { get; set; }

        public NodeConnectionAttribute(string actionName, string targetName, string connectionDescription = null)
        {
            TargetName = targetName;
            ActionName = actionName;
            ConnectionDescription = connectionDescription;
        }
    }
}