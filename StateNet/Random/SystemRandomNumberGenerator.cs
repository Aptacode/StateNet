using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.Random
{
    public class SystemRandomNumberGenerator : IRandomNumberGenerator
    {
        private static readonly System.Random RandomGenerator = new System.Random();

        public int Generate(int min, int max)
        {
            return RandomGenerator.Next(min, max);
        }
    }
}