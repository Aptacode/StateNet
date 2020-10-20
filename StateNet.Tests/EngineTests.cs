using Aptacode.StateNet.Engine;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Random;
using Moq;
using Xunit;

namespace StateNet.Tests
{
    public class EngineTests
    {
        [Fact]
        public void EngineSingleTransition()
        {
            var network = new NetworkBuilder().SetStartState("Start")
                .AddConnection("Start", "Next", "A", _ => 1)
                .Build();

            var engine = new Engine(network, new SystemRandomNumberGenerator());

            var startState = engine.CurrentState;
            var secondState = engine.Apply("Next");

            Assert.Equal("Start", startState);
            Assert.Equal("A", secondState);
        }

        [Fact]
        public void EngineReverseTransition()
        {
            var network = new NetworkBuilder().SetStartState("A")
                .AddConnection("A", "Next", "B", _ => 1)
                .AddConnection("B", "Next", "A", _ => 1)
                .Build();

            var engine = new Engine(network, new SystemRandomNumberGenerator());

            var startState = engine.CurrentState;
            var secondState = engine.Apply("Next");
            var thirdState = engine.Apply("Next");

            Assert.Equal("A", startState);
            Assert.Equal("B", secondState);
            Assert.Equal("A", thirdState);
        }


        [Fact]
        public void EngineSimpleConnectionWeightSelection()
        {
            var network = new NetworkBuilder().SetStartState("A")
                .AddConnection("A", "Next", "B", _ => 1)
                .AddConnection("A", "Next", "C", _ => 1)
                .Build();

            var mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            mockRandomNumberGenerator
                .Setup(r => r.Generate(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            var engine = new Engine(network, mockRandomNumberGenerator.Object);

            var startState = engine.CurrentState;
            var secondState = engine.Apply("Next");

            Assert.Equal("A", startState);
            Assert.Equal("C", secondState);
        }

        [Fact]
        public void EngineTransitionHistory()
        {
            var network = new NetworkBuilder().SetStartState("A")
                .AddConnection("A", "Next", "B", x =>(uint) (x.Transitions.Count == 0 ? 1 : 0))
                .AddConnection("A", "Next", "C", x => (uint)(x.Transitions.Count > 0 ? 1 : 0))
                .AddConnection("B", "Next", "A", _ => 1)
                .AddConnection("C", "Next", "D", _ => 1)
                .Build();

            var engine = new Engine(network, new SystemRandomNumberGenerator());

            var state1 = engine.CurrentState;
            var state2 = engine.Apply("Next");
            var state3 = engine.Apply("Next");
            var state4 = engine.Apply("Next");
            var state5 = engine.Apply("Next");

            Assert.Equal("A", state1);
            Assert.Equal("B", state2);
            Assert.Equal("A", state3);
            Assert.Equal("C", state4);
            Assert.Equal("D", state5);
        }
    }
}
