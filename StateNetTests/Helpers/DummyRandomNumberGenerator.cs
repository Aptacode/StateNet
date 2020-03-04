using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.Tests.Helpers
{
    public class DummyRandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly List<int> _sequence;
        private int _callCount;

        public DummyRandomNumberGenerator(params int[] sequence)
        {
            _callCount = 0;
            _sequence = sequence.ToList();
        }

        public int Generate(int min, int max)
        {
            return _sequence[_callCount++];
        }
    }
}