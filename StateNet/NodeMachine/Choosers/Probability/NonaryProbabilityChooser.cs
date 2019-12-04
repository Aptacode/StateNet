using System;
using Aptacode.StateNet.NodeMachine.Choices;

namespace Aptacode.StateNet.NodeMachine.Choosers.Probability
{
    public class NonaryProbabilityChooser : ProbabilityChooser<NonaryChoice>
    {
        public NonaryProbabilityChooser(int item1Weight,
                                        int item2Weight,
                                        int item3Weight,
                                        int item4Weight,
                                        int item5Weight,
                                        int item6Weight,
                                        int item7Weight,
                                        int item8Weight)
        {
            Item1Weight = item1Weight;
            Item2Weight = item2Weight;
            Item3Weight = item3Weight;
            Item4Weight = item4Weight;
            Item5Weight = item5Weight;
            Item6Weight = item6Weight;
            Item7Weight = item7Weight;
            Item8Weight = item8Weight;
        }

        public override NonaryChoice GetChoice()
        {
            if (TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);
            if (randomChoice <= Item1Weight)
            {
                return NonaryChoice.Item1;
            }
            else if (randomChoice <= Item1Weight + Item2Weight)
            {
                return NonaryChoice.Item2;
            }
            else if (randomChoice <= Item1Weight + Item2Weight + Item3Weight)
            {
                return NonaryChoice.Item3;
            }
            else if (randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight)
            {
                return NonaryChoice.Item4;
            }
            else if (randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight)
            {
                return NonaryChoice.Item5;
            }
            else if (randomChoice <= Item1Weight + Item2Weight + Item3Weight + Item4Weight + Item5Weight + Item6Weight)
            {
                return NonaryChoice.Item6;
            }
            else if (randomChoice <=
              Item1Weight +
              Item2Weight +
              Item3Weight +
              Item4Weight +
              Item5Weight +
              Item6Weight +
              Item7Weight)
            {
                return NonaryChoice.Item7;
            }
            else if (randomChoice <= TotalWeight)
            {
                return NonaryChoice.Item8;
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

        public int Item8Weight { get; set; }

        public int TotalWeight => Item1Weight +
            Item2Weight +
            Item3Weight +
            Item4Weight +
            Item5Weight +
            Item6Weight +
            Item7Weight +
            Item8Weight;
    }
}
