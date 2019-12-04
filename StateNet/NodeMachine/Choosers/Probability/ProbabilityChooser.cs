using Aptacode.StateNet.NodeMachine.Choices;
using System;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probability
{
    public abstract class ProbabilityChooser
    {
        protected static readonly Random RandomGenerator = new Random(DateTime.Now.Millisecond);
    }

    public abstract class ProbabilityChooser<TChoice> : ProbabilityChooser, IChooser<TChoice>
        where TChoice : System.Enum
    {
        public abstract TChoice GetChoice();
    }
}
