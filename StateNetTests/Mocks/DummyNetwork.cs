using Aptacode.StateNet.Attributes;

namespace Aptacode.StateNet.Tests.Mocks
{
    public class DummyNetwork : Network
    {
        private int decision1Count;

        [StateName("D1")]
        [Connection("Next", "D1", "StateCount(\"D2\") < 2 ? 1 : 0")]
        [Connection("Next", "End", "StateCount(\"D2\") >= 2 ? 1 : 0")]
        public State Decision1TestState { get; set; }

        [StateName("D2")]
        [Connection("Next", "D1", "StateCount(\"D2\") > 2 ? 1 : 0")]
        [Connection("Next", "D2", "StateCount(\"D2\") <= 2 ? 1 : 0")]
        public State Decision2TestState { get; set; }

        [StateName("End")] public State EndTestState { get; set; }

        [StartState("Start")]
        [Connection("Left", "D1")]
        [Connection("Right", "D2")]
        public State StartTestState { get; set; }
    }
}