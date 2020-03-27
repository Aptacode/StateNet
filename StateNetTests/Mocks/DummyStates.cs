using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.Mocks
{
    public class DummyStates
    {
        public enum States
        {
            Ready,
            Playing,
            Paused,
            Stopped
        }

        /// <summary>
        ///     Creates a list of States from the given array of strings
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public static IEnumerable<State> Create(params string[] states)
        {
            return states.Select(state => new State(state));
        }
    }
}