using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class Tests
    {
        private Engine _engine;

        private UnaryNode U1;
        private UnaryNode U2;
        private UnaryNode U3;
        private BinaryNode B1;
        private TernaryNode T1;
        private EndNode End1;


        [SetUp]
        public void Setup()
        {
            U1 = new UnaryNode("U1");
            U2 = new UnaryNode("U2");
            U3 = new UnaryNode("U3");
            B1 = new BinaryNode("B1");
            T1 = new TernaryNode("T1");
            End1 = new EndNode("End1");
        }

        private void InstantTransition(Node sender) => sender.Exit();

        [Test]
        public void ValidEngine()
        {
            U1.Visits(End1);
            Assert.IsTrue(new Engine(U1).IsValid());
        }

        [Test]
        public void InvalidEngine()
        {
            U1.Visits(U2);
            U2.Visits(U1);

            Assert.IsFalse(new Engine(U1).IsValid());
        }

        [Test, MaxTime(200)]
        public void UnaryTransitionLog()
        {
            var engine = new Engine(U1);

            U1.Visits(U2);
            U2.Visits(End1);
            U1.OnVisited += InstantTransition;
            U2.OnVisited += InstantTransition;

            engine.Start();

            engine.OnFinished += (s) =>
            {
                Assert.That(engine.GetVisitLog(), Is.EquivalentTo(new List<Node> { U1, U2, End1 }));
            };
        }

        [Test, MaxTime(200)]
        public void TernaryBinaryDistribution()
        {
            var engine = new Engine(T1);

            var distribution = new TernaryDistribution(1, 1, 1);

            T1.Visits(U1, U2, B1, () => new TernaryDistribution(1, 1, 1));
            U1.Visits(T1);
            U2.Visits(T1);
            B1.Visits(T1, End1, () => new BinaryDistribution(1, 1));

            T1.OnVisited += InstantTransition;
            U1.OnVisited += InstantTransition;
            U2.OnVisited += InstantTransition;
            B1.OnVisited += InstantTransition;

            engine.Start();

            engine.OnFinished += (s) =>
            {
                var log = engine.GetVisitLog();
                Assert.Pass();
            };
        }
    }
}
