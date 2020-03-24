using System.Collections.Generic;

namespace Aptacode.StateNet.Tests.Mocks
{
    public class DummyActions
    {
        public enum Actions
        {
            Play,
            Pause,
            Stop
        }

        public static IEnumerable<string> Create(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return i.ToString();
            }
        }
    }
}