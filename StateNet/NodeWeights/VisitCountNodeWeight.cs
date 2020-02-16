using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.NodeWeights
{
    public class VisitCountNodeWeight : INodeWeight
    {
        public int Count { get; set; }
        public int LessThenWeight { get; set; }
        public int EqualToWeight { get; set; }
        public int GreaterThenWeight { get; set; }
        public Node Node { get; set; }
        public VisitCountNodeWeight(Node node, int count, int lessThenWeight, int equalToWeight, int greaterThenWeight)
        {
            Node = node;
            Count = count;
            LessThenWeight = lessThenWeight;
            EqualToWeight = equalToWeight;
            GreaterThenWeight = greaterThenWeight;
        }

        public int GetWeight(List<Node> history)
        {
            var nodeCount = history.Count(n => n == Node);
            if (nodeCount > Count)
            {
                return GreaterThenWeight;
            }
            else if (nodeCount < Count)
            {
                return LessThenWeight;
            }
            else
            {
                return EqualToWeight;
            }
        }
    }
}
