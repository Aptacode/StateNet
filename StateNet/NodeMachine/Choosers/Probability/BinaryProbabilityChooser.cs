using System;
using Aptacode.StateNet.NodeMachine.Choices;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probability
{
    public class BinaryProbabilityChooser : ProbabilityChooser<BinaryChoice>
    {
        public BinaryProbabilityChooser(int item1Weight, int item2Weight)
        {
            Item1Weight = item1Weight;
            Item2Weight = item2Weight;
        }

        public override BinaryChoice GetChoice()
        {
            if (TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);

            if (randomChoice <= Item1Weight)
            {
                return BinaryChoice.Item1;
            }
            else if (randomChoice <= TotalWeight)
            {
                return BinaryChoice.Item2;
            }
            else
            {
                throw new Exception();
            }
        }

        public int Item1Weight { get; set; }

        public int Item2Weight { get; set; }


        public int TotalWeight => Item1Weight + Item2Weight;
    }
}
