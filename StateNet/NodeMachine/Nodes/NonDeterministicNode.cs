using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Choosers.Probability;
using System;

namespace Aptacode.StateNet.NodeMachine.Nodes
{
    public abstract class NonDeterministicNode : Node
    {
        protected NonDeterministicNode(string name) : base(name) { }
    }

    public abstract class NonDeterministicNode<TChoice> : NonDeterministicNode
        where TChoice : System.Enum
    {
        protected NonDeterministicNode(string name) : base(name) => Chooser =
            ProbabilityChooserFactory.GetChooser<TChoice>() ;

        public IChooser<TChoice> Chooser { get; set; }
    }
}
