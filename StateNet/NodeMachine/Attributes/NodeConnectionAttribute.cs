using System;

namespace Aptacode.StateNet.NodeMachine.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class NodeConnectionAttribute : Attribute
    {
        public string ActionName;
        public string TargetName;
        public int ConnectionChance;

        public NodeConnectionAttribute(string actionName, string targetName, int connectionChance = 1)
        {
            TargetName = targetName;
            ActionName = actionName;
            ConnectionChance = connectionChance;
        }
    }
}
