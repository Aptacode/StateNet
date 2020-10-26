using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class MatchTracker
    {
        public MatchTracker(int?[] pattern)
        {
            Pattern = pattern;
        }

        public int?[] Pattern { get; }
        public int CurrentMatchIndex { get; private set; }
        public List<int> MatchTransitionIndexes { get; } = new List<int>();

        public void Add(int transitionIndex, int hashCode)
        {
            if (!IsNextInPattern(hashCode))
            {
                CurrentMatchIndex = 0;
                return;
            }

            if (++CurrentMatchIndex < Pattern.Length)
            {
                return;
            }

            CurrentMatchIndex = 0;
            MatchTransitionIndexes.Add(transitionIndex);
        }

        private bool IsNextInPattern(int hashCode)
        {
            var patternElement1 = Pattern.ElementAt(CurrentMatchIndex);
            if (!patternElement1.HasValue || patternElement1 == hashCode)
            {
                return true;
            }

            CurrentMatchIndex = 0;
            return false;
        }
    }
}