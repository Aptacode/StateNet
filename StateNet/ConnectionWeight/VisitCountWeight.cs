using Aptacode.StateNet.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.NodeWeights
{
    public class VisitCountWeight : IConnectionWeight
    {
        public int Count { get; set; }
        public int LessThenWeight { get; set; }
        public int EqualToWeight { get; set; }
        public int GreaterThenWeight { get; set; }
        public string State { get; set; }

        public VisitCountWeight(string state, int count, int lessThenWeight, int equalToWeight, int greaterThenWeight)
        {
            State = state;
            Count = count;
            LessThenWeight = lessThenWeight;
            EqualToWeight = equalToWeight;
            GreaterThenWeight = greaterThenWeight;
        }

        public int GetWeight(List<State> history)
        {
            var nodeCount = history.Count(n => n.Name == State);
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