using Aptacode.StateNet.Attributes;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    public class TwoStateStartToEndNetwork : Network
    {
        [StartState("Start")] 
        [Connection("Next", "End")]
        public State StartTestState { get; set; }

        [StateName("End")]
        public State EndTestState { get; set; }
    }
}