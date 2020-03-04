using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.ConnectionWeight
{
    public class VisitCountWeight : IConnectionWeight
    {
        /// <summary>
        ///     Determines the probabilistic weight of a transition depending on the number of times the engine has visited a given
        ///     state
        /// </summary>
        /// <param name="state">Check the visit count of this state</param>
        /// <param name="comparisonVisitCount">The number of visits which determines a change in weight</param>
        /// <param name="lessThenWeight">The resultant weight if 'state' has been visited less then comparisonVisitCount</param>
        /// <param name="equalToWeight">The resultant weight if 'state' has been visited exactly then comparisonVisitCount</param>
        /// <param name="greaterThenWeight">The resultant weight if 'state' has been visited more then comparisonVisitCount</param>
        public VisitCountWeight(string state, int comparisonVisitCount, int lessThenWeight, int equalToWeight,
            int greaterThenWeight)
        {
            State = state;
            ComparisonVisitCount = comparisonVisitCount;
            LessThenWeight = lessThenWeight;
            EqualToWeight = equalToWeight;
            GreaterThenWeight = greaterThenWeight;
        }

        public int ComparisonVisitCount { get; set; }
        public int LessThenWeight { get; set; }
        public int EqualToWeight { get; set; }
        public int GreaterThenWeight { get; set; }
        public string State { get; set; }

        public int GetWeight(List<State> history)
        {
            var nodeCount = GetTotalVisitCount(history);

            if (nodeCount > ComparisonVisitCount)
            {
                return GreaterThenWeight;
            }

            if (nodeCount < ComparisonVisitCount)
            {
                return LessThenWeight;
            }

            return EqualToWeight;
        }

        public int GetTotalVisitCount(List<State> history)
        {
            return history.Count(n => n.Name == State);
        }
    }
}