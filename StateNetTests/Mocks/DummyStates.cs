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

        public static IEnumerable<State> Create(params string[] states) => states.Select(state => new State(state));
    }
}