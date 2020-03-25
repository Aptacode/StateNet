using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Random;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class EngineTests
    {
        private IStateNetwork GetTestNetwork()
        {
            IStateNetwork stateNetwork = new StateNetwork();
            var networkEditor = new StateNetworkEditor(stateNetwork);

            networkEditor.SetStart("ready");

            networkEditor.Always("ready", "Play", "playing");
            networkEditor.Always("ready", "Stop", "stopped");
            networkEditor.Always("playing", "Pause", "paused");
            networkEditor.Always("playing", "Stop", "stopped");
            networkEditor.Always("paused", "Play", "playing");
            networkEditor.Always("paused", "Stop", "stopped");

            return stateNetwork;
        }

        [Test]
        public void EngineLogTests()
        {
            var engine = new StateNetEngine(new SystemRandomNumberGenerator(), GetTestNetwork());

            engine.Start();
            engine.Apply("Play");
            engine.Apply("Pause");
            engine.Apply("Play");
            engine.Apply("Stop");

            var expectedStateLog = new List<string>
            {
                "ready", "playing", "paused", "playing", "stopped"
            };

            var expectedInputLog = new List<string>
            {
                "Play", "Pause", "Play", "Stop"
            };

            Assert.That(() => engine.History.States.Select(state => state.Name),
                Is.EquivalentTo(expectedStateLog).After(200).MilliSeconds.PollEvery(1).MilliSeconds);

            Assert.That(() => engine.History.Inputs.Select(input => input.Name),
                Is.EquivalentTo(expectedInputLog).After(200).MilliSeconds.PollEvery(1).MilliSeconds);
        }
    }
}