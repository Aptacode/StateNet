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
        public static EngineLog Generate(params int[] sequence)
        {
            var log = new EngineLog();
            sequence.Select(v => (Input.Empty, new State(v.ToString()))).ToList()
                .ForEach(pair => log.Add(pair.Empty, pair.Item2));
            return log;
        }
    }
}