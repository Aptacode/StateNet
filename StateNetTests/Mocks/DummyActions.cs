using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.Mocks
{
    public class DummyInputs
    {
        public enum Actions
        {
            Play,
            Pause,
            Stop
        }

        public static IEnumerable<Input> Create(params string[] inputs)
        {
            return inputs.Select(input => new Input(input));
        }
    }
}