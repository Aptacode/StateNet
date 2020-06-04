using Aptacode.StateNet.Engine.History;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.Helpers
{
    public static class StateHistoryGenerator
    {
        /// <summary>
        ///     Returns a list of states generated from the sequence of integers passed
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEngineHistory Generate(params int[] sequence)
        {
            var history = new EngineHistory();

            if (sequence.Length == 0) return history;

            history.SetStart(new State(sequence[0].ToString()));

            for (var i = 1; i < sequence.Length; i++)
                history.Log(new State((i - 1).ToString()), new Input("next"), new State(i.ToString()));

            return history;
        }
    }
}