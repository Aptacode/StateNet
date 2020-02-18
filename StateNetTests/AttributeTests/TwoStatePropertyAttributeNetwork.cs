using Aptacode.StateNet.Attributes;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    public class TwoStatePropertyAttributeNetwork : Network
    {
        [StartState("Start")]
        [Connection("Next", "Private")]
        public State StartTestState { get; }

        [StateName("End")] public State EndTestState { get; }

        [StateName("Private")]
        [Connection("Finish", "End")]
        public State PrivateState { get; set; }
    }
}