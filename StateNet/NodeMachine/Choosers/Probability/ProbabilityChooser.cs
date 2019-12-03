using Aptacode.StateNet.NodeMachine.Choices;
using System;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probability
{
    public abstract class ProbabilityChooser<TChoice> : IChooser<TChoice>
        where TChoice : System.Enum
    {
        protected static readonly Random RandomGenerator = new Random(DateTime.Now.Millisecond);

        public abstract TChoice GetChoice();
    }
}
