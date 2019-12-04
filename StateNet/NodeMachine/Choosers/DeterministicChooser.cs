using Aptacode.StateNet.NodeMachine.Choices;

namespace Aptacode.StateNet.NodeMachine.Choosers
{
    public class DeterministicChooser<TChoice> : IChooser<TChoice>
        where TChoice : System.Enum
    {
        public DeterministicChooser(TChoice choice) => Choice = choice;

        public TChoice GetChoice() => Choice;

        public TChoice Choice { get; set; }
    }
}
