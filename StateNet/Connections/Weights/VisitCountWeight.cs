using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Connections.Weights
{
    public class VisitCountWeight : ConnectionWeight
    {
        private int _equalToWeight;
        private int _greaterThenWeight;

        private int _lessThenWeight;

        public VisitCountWeight()
        {
            State = null;
            ComparisonVisitCount = 0;
            LessThenWeight = 0;
            EqualToWeight = 0;
            GreaterThenWeight = 0;
        }

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

        /// <summary>
        ///     Count this states visits
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     Number to compare the TotalVisitCount against
        /// </summary>
        public int ComparisonVisitCount { get; set; }

        /// <summary>
        ///     The weight returned if the TotalVisitCount is less then the ComparisonVisitCount
        /// </summary>
        /// private int _weight;
        public int LessThenWeight
        {
            get => _lessThenWeight;
            set => _lessThenWeight = value >= 0 ? value : 0;
        }

        /// <summary>
        ///     The weight returned if the TotalVisitCount equals the ComparisonVisitCount
        /// </summary>
        /// private int _weight;
        public int EqualToWeight
        {
            get => _equalToWeight;
            set => _equalToWeight = value >= 0 ? value : 0;
        }

        /// <summary>
        ///     The weight returned if the TotalVisitCount is greater then the ComparisonVisitCount
        /// </summary>
        /// private int _weight;
        public int GreaterThenWeight
        {
            get => _greaterThenWeight;
            set => _greaterThenWeight = value >= 0 ? value : 0;
        }

        public override string TypeName => nameof(VisitCountWeight);

        /// <summary>
        ///     Returns the ConnectionWeight based on the stateHistory
        /// </summary>
        /// <param name="stateHistory"></param>
        /// <returns></returns>
        public override int GetWeight(List<(Input, State)> stateHistory)
        {
            var nodeCount = GetTotalVisitCount(stateHistory);

            if (nodeCount > ComparisonVisitCount)
            {
                return GreaterThenWeight;
            }

            return nodeCount < ComparisonVisitCount ? LessThenWeight : EqualToWeight;
        }

        public override string ToString()
        {
            return $"{TypeName}:{State},{ComparisonVisitCount},{LessThenWeight},{EqualToWeight},{GreaterThenWeight}";
        }

        /// <summary>
        ///     Returns the number of times State has been visited in the history
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public int GetTotalVisitCount(List<(Input, State)> history)
        {
            return history?.Count(n => n.Item2.Name == State) ?? 0;
        }

        public override int GetHashCode()
        {
            return (State, LessThenWeight, EqualToWeight, GreaterThenWeight).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is VisitCountWeight other && Equals(other);
        }

        public bool Equals(VisitCountWeight other)
        {
            return other != null && State.Equals(other.State) && LessThenWeight.Equals(other.LessThenWeight) &&
                   EqualToWeight.Equals(other.EqualToWeight) && GreaterThenWeight.Equals(other.GreaterThenWeight);
        }
    }
}