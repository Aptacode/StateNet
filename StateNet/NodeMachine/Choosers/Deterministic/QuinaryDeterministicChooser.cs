using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using Aptacode.StateNet.NodeMachine.Options;
using System;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine.Choosers.Deterministic
{
    public class QuinaryDeterministicChooser : DeterministicChooser<QuinaryChoice>
    {
        public QuinaryDeterministicChooser(Node option1, Node option2, Node option3, Node option4, Node option5) : base(new QuinaryOptions(option1,
                                                                                                                                           option2,
                                                                                                                                           option3,
                                                                                                                                           option4,
                                                                                                                                           option5),
                                                                                                                        QuinaryChoice.Item1)
        { }
    }
}
