using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    public class TwoStateStartToEndNetwork : StateNetwork
    {
        [StartState("Start")]
        [Connection("Next", "End")]
        public State StartTestState { get; set; }

        [StateName("End")] public State EndTestState { get; set; }
    }
}