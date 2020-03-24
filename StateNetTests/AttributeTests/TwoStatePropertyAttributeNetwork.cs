using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    public class TwoStatePropertyAttributeNetwork : StateNetwork
    {
        [StartState("Start")]
        [Connection("Next", "Private")]
        public State StartTestState { get; set; }

        [StateName("End")] public State EndTestState { get; set; }

        [StateName("Private")]
        [Connection("Finish", "End")]
        public State PrivateState { get; set; }
    }
}