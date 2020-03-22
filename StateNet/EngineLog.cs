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

        public int StateVisitCount(string name)
        {
            return Log.Count(pair => pair.Item2.Name == name);
        }
        public int InputAppliedCount(string name)
        {
            return Log.Count(pair => pair.Item1.Name == name);
        }

        public int TransitionInCount(string inputName, string stateName)
        {
            var count = 0;

            if (Log.Count < 1)
                return count;

            for (var i = 1; i < Log.Count; i++)
            {
                if(Log[i].Item2.Name == stateName && Log[i - 1].Item1.Name == inputName)
                {
                    count++;
                }
            }
            return count;
        }

        public int TransitionOutCount(string stateName, string inputName)
        {
            return Log.Count(pair => pair.Item1.Name == inputName && pair.Item2.Name == stateName);
        }


    }
}