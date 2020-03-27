using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.Mocks
{
    public class DummyInputs
    {
        public enum PlayerInputs
        {
            Play,
            Pause,
            Stop
        }

        /// <summary>
        ///     Returns a list of Inputs from a collection of strings
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public static IEnumerable<Input> Create(params string[] inputs)
        {
            return inputs.Select(input => new Input(input));
        }
    }
}