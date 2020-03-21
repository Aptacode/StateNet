using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet
{
    public class EngineLog
    {
        public readonly List<(Input, State)> Log;

        public EngineLog()
        {
            Log = new List<(Input, State)>();
        }

        public IEnumerable<Input> InputLog => Log.Select(pair => pair.Item1);
        public IEnumerable<State> StateLog => Log.Select(pair => pair.Item2);

        public void Add(Input input, State state)
        {
            Log.Add((input, state));
        }

        public int StateCount(string name)
        {
            return Log.Count(pair => pair.Item2.Name == name);
        }

        public int InputCount(string name)
        {
            return Log.Count(pair => pair.Item1.Name == name);
        }
    }
}