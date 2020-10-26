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
    }
}