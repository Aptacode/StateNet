using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Random;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class EngineTests
    {
        private Network GetTestNetwork()
        {
            var network = new Network();

            network.SetStart("ready");

            network.Always("ready", "Play", "playing");
            network.Always("ready", "Stop", "stopped");
            network.Always("playing", "Pause", "paused");
            network.Always("playing", "Stop", "stopped");
            network.Always("paused", "Play", "playing");
            network.Always("paused", "Stop", "stopped");

            return network;
        }

        [Test]
        public void EngineLogTests()
        {
            var engine = new Engine(new SystemRandomNumberGenerator(), GetTestNetwork());

            engine.Start();
            engine.Apply("Play");
            engine.Apply("Pause");
            engine.Apply("Play");
            engine.Apply("Stop");

            var expectedLog = new List<(string, string)>
                {("", "ready"), ("Play", "playing"), ("Pause", "paused"), ("Play", "playing"), ("Stop", "stopped")};

            Assert.That(() => engine.GetLog().Log.Select(pair => (pair.Item1.Name, pair.Item2.Name)),
                Is.EquivalentTo(expectedLog).After(200).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}