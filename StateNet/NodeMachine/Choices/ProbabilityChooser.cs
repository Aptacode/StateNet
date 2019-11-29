using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public abstract class ProbabilityChooser<TChoice> : IChooser<TChoice>
        where TChoice : System.Enum
    {
        protected static readonly Random RandomGenerator = new Random();

        public abstract TChoice GetChoice();
    }
}
