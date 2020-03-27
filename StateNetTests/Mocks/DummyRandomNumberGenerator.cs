using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.Tests.Mocks
{
    /// <summary>
    ///     Mock random number generator which returns a pre determined sequence of numbers
    /// </summary>
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
            if (_callCount >= _sequence.Count)
            {
                _callCount = 0;
            }

            return _sequence.ElementAt(_callCount++);
        }
    }
}