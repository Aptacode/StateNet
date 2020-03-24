using System.Collections.Generic;

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

        public static IEnumerable<string> Create(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return i.ToString();
            }
        }

    }
}