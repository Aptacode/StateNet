using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public class QuaternaryDistribution : Distribution<QuaternaryChoice>
    {
        public QuaternaryDistribution(int item1Weight, int item2Weight, int item3Weight, int item4Weight)
        {
            Item1Weight = item1Weight;
            Item2Weight = item2Weight;
            Item3Weight = item3Weight;
            Item4Weight = item4Weight;
        }

        public override QuaternaryChoice GetChoice()
        {
            if(TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);
            if(randomChoice <= Item1Weight)
            {
                return QuaternaryChoice.Item1;
            } else if(randomChoice <= Item1Weight + Item2Weight)
            {
                return QuaternaryChoice.Item2;
            } else if(randomChoice <= Item1Weight + Item2Weight + Item3Weight)
            {
                return QuaternaryChoice.Item3;
            }  else if(randomChoice <= TotalWeight)
            {
                return QuaternaryChoice.Item4;
            } else
            {
                throw new Exception();
            }
        }

        public int Item1Weight { get; set; }

        public int Item2Weight { get; set; }

        public int Item3Weight { get; set; }
        public int Item4Weight { get; set; }

        public int TotalWeight => Item1Weight + Item2Weight + Item3Weight + Item4Weight;
    }
}
