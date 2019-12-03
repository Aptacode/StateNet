using Aptacode.StateNet.NodeMachine;
using Aptacode.StateNet.NodeMachine.Choices;
using Aptacode.StateNet.NodeMachine.Choosers;
using Aptacode.StateNet.NodeMachine.Choosers.Probability;
using Aptacode.StateNet.NodeMachine.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Tests.NodeMachine
{
    public class Tests
    {
        private BinaryNode B1;
        private EndNode End1;
        private TernaryNode T1;

        private UnaryNode U1;
        private UnaryNode U2;
        private UnaryNode U3;

        private void InstantTransition(Node sender) => sender.Exit();

        [Test]
        public void InvalidEngine()
        {
            U1.Visits(U2);
            U2.Visits(U1);

            Assert.IsFalse(new NodeEngine(U1).IsValid());
        }


        [SetUp]
        public void Setup()
        {
            U1 = new UnaryNode(nameof(U1));
            U2 = new UnaryNode(nameof(U2));
            U3 = new UnaryNode(nameof(U3));
            B1 = new BinaryNode(nameof(B1));
            T1 = new TernaryNode(nameof(T1));
            End1 = new EndNode(nameof(End1));
        }

        [Test, MaxTime(200)]
        public void TernaryBinaryDistribution()
        {
            var engine = new NodeEngine(T1);

            T1.Visits(U1, U2, B1, new TernaryProbabilityChooser(1, 1, 1));
            U1.Visits(T1);
            U2.Visits(T1);
            B1.Visits(T1, End1, new DeterministicChooser<BinaryChoice>(BinaryChoice.Item1));

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

        [Test, MaxTime(200)]
        public void UnaryTransitionLog()
        {
            var engine = new NodeEngine(U1);

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

        [Test]
        public void ValidEngine()
        {
            U1.Visits(End1);
            Assert.IsTrue(new NodeEngine(U1).IsValid());
        }
    }
}
