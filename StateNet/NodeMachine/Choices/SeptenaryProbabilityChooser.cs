using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public class SeptenaryProbabilityChooser : ProbabilityChooser<SeptenaryChoice>
    {
        public SeptenaryProbabilityChooser(int item1Weight,
                                         int item2Weight,
                                         int item3Weight,
                                         int item4Weight,
                                         int item5Weight,
                                         int item6Weight,
                                         int item7Weight)
        {
            Item1Weight = item1Weight;
            Item2Weight = item2Weight;
            Item3Weight = item3Weight;
            Item4Weight = item4Weight;
            Item5Weight = item5Weight;
            Item6Weight = item6Weight;
            Item7Weight = item7Weight;
        }

        public override SeptenaryChoice GetChoice()
        {
            if(TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);
            if(randomChoice <= Item1Weight)
            {
                return SeptenaryChoice.Item1;
            } else if(randomChoice <= Item1Weight + Item2Weight)
            {
                return SeptenaryChoice.Item2;
            } else if(randomChoice <= Item1Weight + Item2Weight + Item3Weight)
            {
                return SeptenaryChoice.Item3;
            } else if(randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight)
            {
                return SeptenaryChoice.Item4;
            }
            else if (randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight)
            {
                return SeptenaryChoice.Item5;
            }
            else if (randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight + Item6Weight)
            {
                return SeptenaryChoice.Item6;
            }
            else if (randomChoice <= TotalWeight)
            {
                return SeptenaryChoice.Item7;
            }
            else
            {
                throw new Exception();
            }
        }

        public int Item1Weight { get; set; }
        public int Item2Weight { get; set; }
        public int Item3Weight { get; set; }
        public int Item4Weight { get; set; }
        public int Item5Weight { get; set; }
        public int Item6Weight { get; set; }
        public int Item7Weight { get; set; }

        public int TotalWeight => Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight + Item6Weight + Item7Weight;
    }
}
