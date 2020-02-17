using Aptacode.StateNet.Events.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aptacode.StateNet.Tests.AttributeTests
{
    public class TwoStateStartToEndNetwork : Network
    {
        [StartState("Start")]
        [Connection("Next", "End")]
        public State StartTestState;

        [StateName("End")]
        public State EndTestState;
    }
}
