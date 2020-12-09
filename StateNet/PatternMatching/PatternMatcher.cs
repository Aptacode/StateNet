using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.PatternMatching
{
    public class PatternMatcher
    {
        public readonly List<int> MatchList = new List<int>();

        public readonly Pattern Pattern;

        public PatternMatcher(Pattern pattern)
        {
            Pattern = pattern;
        }

        public int PatternIndex { get; private set; }

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
            if (patternElement is not null && patternElement == hashCode)
            {
                return true;
            }

            PatternIndex = 0;
            return false;
        }
    }
}