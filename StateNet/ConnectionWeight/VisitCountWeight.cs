using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.ConnectionWeight
{
    public class VisitCountWeight : IConnectionWeight
    {
        public VisitCountWeight(string state, int count, int lessThenWeight, int equalToWeight, int greaterThenWeight)
        {
            State = state;
            Count = count;
            LessThenWeight = lessThenWeight;
            EqualToWeight = equalToWeight;
            GreaterThenWeight = greaterThenWeight;
        }

        public int Count { get; set; }
        public int LessThenWeight { get; set; }
        public int EqualToWeight { get; set; }
        public int GreaterThenWeight { get; set; }
        public string State { get; set; }

        public int GetWeight(List<State> history)
        {
            var nodeCount = history.Count(n => n.Name == State);
            if (nodeCount > Count)
            {
                return GreaterThenWeight;
            }

            if (nodeCount < Count)
            {
                return LessThenWeight;
            }

            return EqualToWeight;
        }
    }
}