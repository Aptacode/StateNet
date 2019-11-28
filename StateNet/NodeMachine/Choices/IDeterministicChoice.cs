using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public class IDeterministicChoice<TChoice> : IChooser<TChoice>
        where TChoice : System.Enum
    {
        public IDeterministicChoice(TChoice choice) => Choice = choice;

        public TChoice GetChoice() => Choice ;

        public TChoice Choice { get; }
    }
}
