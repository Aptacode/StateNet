using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public class TernaryDistribution : Distribution<TernaryChoice>
    {
        public TernaryDistribution(int item1Weight, int item2Weight, int item3Weight)
        {
            Item1Weight = item1Weight;
            Item2Weight = item2Weight;
            Item3Weight = item3Weight;
        }

        public override TernaryChoice GetChoice()
        {
            if(TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);
            if(randomChoice <= Item1Weight)
            {
                return TernaryChoice.Item1;
            } else if(randomChoice <= Item1Weight + Item2Weight)
            {
                return TernaryChoice.Item2;
            } else if(randomChoice <= TotalWeight)
            {
                return TernaryChoice.Item3;
            } else
            {
                throw new Exception();
            }
        }

        public int Item1Weight { get; set; }

        public int Item2Weight { get; set; }

        public int Item3Weight { get; set; }

        public int TotalWeight => Item1Weight + Item2Weight + Item3Weight;
    }
}
