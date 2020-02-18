using Aptacode.StateNet.Attributes;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    public class TwoStatePropertyAttibuteNetwork : Network
    {
        [StartState("Start")]
        [Connection("Next", "Private")]
        public State StartTestState { get; private set; }

        [StateName("End")] public State EndTestState { get; private set; }

        [StateName("Private")]
        [Connection("Finish", "End")]
        private State PrivateState { get; set; }
    }
}