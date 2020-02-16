using System;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.NodeWeights;

namespace Aptacode.StateNet.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class NodeConnectionAttribute : Attribute
    {
        public string ActionName;
        public string TargetName;
        public string ConnectionDescription;

        public NodeConnectionAttribute(string actionName, string targetName, string connectionDescription = null)
        {
            TargetName = targetName;
            ActionName = actionName;
            ConnectionDescription = connectionDescription;
        }
    }
}
