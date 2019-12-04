using Aptacode.StateNet.NodeMachine.Choices;
using System;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probability
{
    public static class ProbabilityChooserFactory
    {
        public static ProbabilityChooser<TChoice> GetChooser<TChoice>()
            where TChoice : System.Enum
        {
            var type = typeof(TChoice);
            if(type == typeof(BinaryChoice))
            {
                return new BinaryProbabilityChooser(1, 1) as ProbabilityChooser<TChoice>;
            } else if(type == typeof(BinaryChoice))
            {
                return new TernaryProbabilityChooser(1, 1, 1) as ProbabilityChooser<TChoice>;
            } else if(type == typeof(QuaternaryChoice))
            {
                return new QuaternaryProbabilityChooser(1, 1, 1, 1) as ProbabilityChooser<TChoice>;
            } else if(type == typeof(QuinaryChoice))
            {
                return new QuinaryProbabilityChooser(1, 1, 1, 1, 1) as ProbabilityChooser<TChoice>;
            } else if(type == typeof(SenaryChoice))
            {
                return new SenaryProbabilityChooser(1, 1, 1, 1, 1, 1) as ProbabilityChooser<TChoice>;
            } else if(type == typeof(SeptenaryChoice))
            {
                return new SeptenaryProbabilityChooser(1, 1, 1, 1, 1, 1, 1) as ProbabilityChooser<TChoice>;
            } else if(type == typeof(OctaryChoice))
            {
                return new OctaryProbabilityChooser(1, 1, 1, 1, 1, 1, 1, 1) as ProbabilityChooser<TChoice>;
            } else if(type == typeof(NonaryChoice))
            {
                return new NonaryProbabilityChooser(1, 1, 1, 1, 1, 1, 1, 1, 1) as ProbabilityChooser<TChoice>;
            } else
            {
                return null;
            }
        }
    }
}
