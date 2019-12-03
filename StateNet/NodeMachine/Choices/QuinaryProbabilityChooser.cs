using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public class QuinaryProbabilityChooser : ProbabilityChooser<QuinaryChoice>
    {
        public QuinaryProbabilityChooser(int item1Weight,
                                         int item2Weight,
                                         int item3Weight,
                                         int item4Weight,
                                         int item5Weight)
        {
            Item1Weight = item1Weight;
            Item2Weight = item2Weight;
            Item3Weight = item3Weight;
            Item4Weight = item4Weight;
            Item5Weight = item5Weight;
        }

        public override QuinaryChoice GetChoice()
        {
            if(TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);
            if(randomChoice <= Item1Weight)
            {
                return QuinaryChoice.Item1;
            } else if(randomChoice <= Item1Weight + Item2Weight)
            {
                return QuinaryChoice.Item2;
            } else if(randomChoice <= Item1Weight + Item2Weight + Item3Weight)
            {
                return QuinaryChoice.Item3;
            } else if(randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight)
            {
                return QuinaryChoice.Item4;
            } else if(randomChoice <= TotalWeight)
            {
                return QuinaryChoice.Item5;
            } else
            {
                throw new Exception();
            }
        }

        public int Item1Weight { get; set; }

        public int Item2Weight { get; set; }

        public int Item3Weight { get; set; }

        public int Item4Weight { get; set; }

        public int Item5Weight { get; set; }

        public int TotalWeight => Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight;
    }
}
