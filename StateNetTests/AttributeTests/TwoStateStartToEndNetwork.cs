using Aptacode.StateNet.Attributes;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    public class TwoStateStartToEndNetwork : Network
    {
        [StateName("End")] public State EndTestState;

        [StartState("Start")] [Connection("Next", "End")]
        public State StartTestState;
    }
}