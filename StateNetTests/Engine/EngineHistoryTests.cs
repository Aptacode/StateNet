using System.Linq;
using Aptacode.StateNet.Engine.History;
using Aptacode.StateNet.Network;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.Engine
{
    public class EngineHistoryTests
    {
        [Test]
        public void Log()
        {
            var engineHistory = new EngineHistory();
            var a = new State("a");
            var b = new State("b");
            var c = new State("c");

            var next = new Input("next");
            var back = new Input("back");

            Assert.IsTrue(!engineHistory.Inputs.Any());
            Assert.IsTrue(!engineHistory.States.Any());
            Assert.IsNull(engineHistory.StartState);
            Assert.AreEqual(0, engineHistory.StateVisitCount(a.Name));
            Assert.AreEqual(0, engineHistory.StateVisitCount(b.Name));
            Assert.AreEqual(0, engineHistory.StateVisitCount(c.Name));
            Assert.AreEqual(0, engineHistory.InputAppliedCount(next.Name));
            Assert.AreEqual(0, engineHistory.InputAppliedCount(back.Name));

            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, a.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(next.Name, a.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, b.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(next.Name, b.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, c.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(next.Name, c.Name));

            engineHistory.SetStart(a);
            Assert.IsTrue(!engineHistory.Inputs.Any());
            Assert.IsTrue(engineHistory.States.Count() == 1); 
            Assert.AreEqual(a, engineHistory.StartState);
            Assert.AreEqual(1, engineHistory.StateVisitCount(a.Name));
            Assert.AreEqual(0, engineHistory.StateVisitCount(b.Name));
            Assert.AreEqual(0, engineHistory.StateVisitCount(c.Name));
            Assert.AreEqual(0, engineHistory.InputAppliedCount(next.Name));
            Assert.AreEqual(0, engineHistory.InputAppliedCount(back.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, a.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(a.Name, next.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, b.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(b.Name, next.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, c.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(c.Name, next.Name));

            engineHistory.Log(a, next, b);
            Assert.IsTrue(engineHistory.Inputs.Count() == 1);
            Assert.IsTrue(engineHistory.States.Count() == 2);
            Assert.AreEqual(1, engineHistory.StateVisitCount(a.Name));
            Assert.AreEqual(1, engineHistory.StateVisitCount(b.Name));
            Assert.AreEqual(0, engineHistory.StateVisitCount(c.Name));
            Assert.AreEqual(1, engineHistory.InputAppliedCount(next.Name));
            Assert.AreEqual(0, engineHistory.InputAppliedCount(back.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, a.Name));
            Assert.AreEqual(1, engineHistory.TransitionOutCount(a.Name, next.Name));
            Assert.AreEqual(1, engineHistory.TransitionInCount(next.Name, b.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(b.Name, next.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, c.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(c.Name, next.Name));
         
            engineHistory.Log(b, back, c);
            Assert.IsTrue(engineHistory.Inputs.Count() == 2);
            Assert.IsTrue(engineHistory.States.Count() == 3);
            Assert.AreEqual(1, engineHistory.StateVisitCount(a.Name));
            Assert.AreEqual(1, engineHistory.StateVisitCount(b.Name));
            Assert.AreEqual(1, engineHistory.StateVisitCount(c.Name));
            Assert.AreEqual(1, engineHistory.InputAppliedCount(next.Name));
            Assert.AreEqual(1, engineHistory.InputAppliedCount(back.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, a.Name));
            Assert.AreEqual(1, engineHistory.TransitionOutCount(a.Name, next.Name));
            Assert.AreEqual(1, engineHistory.TransitionInCount(next.Name, b.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(b.Name, next.Name));
            Assert.AreEqual(0, engineHistory.TransitionInCount(next.Name, c.Name));
            Assert.AreEqual(0, engineHistory.TransitionOutCount(c.Name, next.Name));
        }
    }
}