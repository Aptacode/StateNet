using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers
{
    public class ProbabilisticChooser<TChoices> : NodeChooser<TChoices>
        where TChoices : System.Enum
    {
        protected static readonly Random RandomGenerator = new Random(DateTime.Now.Millisecond);

        private readonly Dictionary<TChoices, int> _distribution = ((TChoices[])Enum.GetValues(typeof(TChoices))).ToDictionary(Choice => Choice,
                                                                                                                               _ => 1);

        public ProbabilisticChooser(NodeOptions<TChoices> options) : base(options) => TotalWeight = _distribution.Count;

        public override Node GetNext()
        {
            if(TotalWeight == 0)
            {
                throw new Exception();
            }

            var randomChoice = RandomGenerator.Next(1, TotalWeight + 1);

            var weightSum = 0;
            foreach(var keyValue in _distribution)
            {
                weightSum += keyValue.Value;
                if(weightSum >= randomChoice)
                {
                    return Options.GetNode(keyValue.Key);
                }
            }

            throw new Exception();
        }

        public int GetWeight(TChoices choice) => _distribution[choice] ;

        public void SetWeight(TChoices choice, int weight)
        {
            TotalWeight += weight - _distribution[choice];
            _distribution[choice] = weight;
        }

        public int TotalWeight { get; private set; }
    }
}
