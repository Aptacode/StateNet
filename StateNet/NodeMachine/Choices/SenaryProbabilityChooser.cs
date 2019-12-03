using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public class SenaryProbabilityChooser : ProbabilityChooser<SenaryChoice>
    {
        public SenaryProbabilityChooser(int item1Weight,
                                         int item2Weight,
                                         int item3Weight,
                                         int item4Weight,
                                         int item5Weight,
                                         int item6Weight)
        {
            Item1Weight = item1Weight;
            Item2Weight = item2Weight;
            Item3Weight = item3Weight;
            Item4Weight = item4Weight;
            Item5Weight = item5Weight;
            Item6Weight = item6Weight;
        }

        public override SenaryChoice GetChoice()
        {
            if(TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);
            if(randomChoice <= Item1Weight)
            {
                return SenaryChoice.Item1;
            } else if(randomChoice <= Item1Weight + Item2Weight)
            {
                return SenaryChoice.Item2;
            } else if(randomChoice <= Item1Weight + Item2Weight + Item3Weight)
            {
                return SenaryChoice.Item3;
            } else if(randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight)
            {
                return SenaryChoice.Item4;
            }
            else if (randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight)
            {
                return SenaryChoice.Item5;
            }
            else if (randomChoice <= TotalWeight)
            {
                return SenaryChoice.Item6;
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

        public int TotalWeight => Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight + Item6Weight;
    }
}
