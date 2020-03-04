using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Tests.Helpers
{
    public static class StateHistoryGenerator
    {
        /// <summary>
        ///     Returns a list of states generated from the sequence of integers passed
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static List<State> Generate(params int[] sequence)
        {
            return sequence.Select(v => new State(v.ToString())).ToList();
        }
    }
}