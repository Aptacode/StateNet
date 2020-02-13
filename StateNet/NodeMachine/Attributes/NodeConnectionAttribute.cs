using System;
using System.Collections.Generic;
using System.Text;

namespace Aptacode.StateNet.NodeMachine.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NodeConnectionAttribute : Attribute
    {
        public string SourceName;
        public string TargetName;
        public double ConnectionChance;

        public NodeConnectionAttribute(string sourceName, string targetName, double connectionChance)
        {
            SourceName = sourceName;
            TargetName = targetName;
            ConnectionChance = connectionChance;
        }
    }
}
