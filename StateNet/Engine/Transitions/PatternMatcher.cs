using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Engine.Transitions
{
    public class PatternMatcher
    {
        public PatternMatcher(int?[] pattern)
        {
            Pattern = pattern;
        }

        public int?[] Pattern { get; }
        public int PatternIndex { get; private set; }
        public List<int> MatchList { get; } = new List<int>();

        public void Add(int index, int hashCode)
        {
            if (!IsNextInPattern(hashCode))
            {
                PatternIndex = 0;
                return;
            }

            if (++PatternIndex < Pattern.Length)
            {
                return;
            }

            PatternIndex = 0;
            MatchList.Add(index);
        }

        private bool IsNextInPattern(int hashCode)
        {
            var patternElement = Pattern.ElementAt(PatternIndex);
            if (!patternElement.HasValue || patternElement == hashCode)
            {
                return true;
            }

            PatternIndex = 0;
            return false;
        }
    }
}