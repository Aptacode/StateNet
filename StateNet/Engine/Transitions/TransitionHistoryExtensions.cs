using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Engine.Transitions
{
    public static class TransitionHistoryExtensions
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
        {
            var tempList = source as T[] ?? source.ToArray();
            return tempList.Skip(Math.Max(0, tempList.Length - count));
        }

        public static IEnumerable<int> ToHomogeneousTransitionHistory(this IEnumerable<string> transitionHistory)
        {
            return transitionHistory.Select(x => x.GetHashCode());
        }

        public static int MatchCount(this IEnumerable<int> transitionHistory, IEnumerable<int?> transitionPattern)
        {
            var matchCount = 0;
            var matchIndex = 0;

            foreach (var element in transitionHistory)
            {
                var comparison = transitionPattern.ElementAt(matchIndex);
                if (comparison == element || comparison == null)
                {
                    matchIndex++;
                    if (matchIndex < transitionPattern.Count())
                    {
                        continue;
                    }

                    matchIndex = 0;
                    matchCount++;
                }
                else
                {
                    matchIndex = 0;
                }
            }

            return matchCount;
        }
    }
}