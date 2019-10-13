using Aptacode.StateNet.Core;
using Aptacode.StateNet.Core.StateTransitionTable;
using Aptacode.StateNet.Core.TransitionResult;
using Aptacode.StateNet.Core.Transitions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.StateNet.Core_Tests
{
    public class StateMachineTests
    {
        public enum States { Begin, Playing, Paused, End };
        public enum Input { Play, Pause, Stop };

        private StateMachine stateMachine;
        private bool canPlay;

        [SetUp]
        public void Setup()
        {
            canPlay = true;

            stateMachine = new StateMachine(StateCollection.FromEnum<States>(), InputCollection.FromEnum<Input>(), new DictionaryStateTransitionTable(), States.Begin.ToString());

            stateMachine.Define(new BinaryTransition(States.Begin.ToString(), Input.Play.ToString(), States.Playing.ToString(), States.End.ToString(), () =>
            {

                if (canPlay)
                {
                    return new BinaryTransitionResult(BinaryChoice.Left, "Started Playing");
                }
                else
                {
                    return new BinaryTransitionResult(BinaryChoice.Right, "Could not start playing");
                }
            }, "Start Playing"));

            stateMachine.Define(new InvalidTransition(States.Begin.ToString(), Input.Pause.ToString(), "Must be Playing to Pause"));

            stateMachine.Define(new UnaryTransition(States.Begin.ToString(), Input.Stop.ToString(), States.End.ToString(), () =>
            {
                return new UnaryTransitionResult("Stopped");
            }, "Stop before playing"));

            stateMachine.Define(new UnaryTransition(States.Playing.ToString(), Input.Play.ToString(), States.Playing.ToString(), () =>
            {
                return new UnaryTransitionResult("Kept playing");
            }, "Already Playing"));

            stateMachine.Define(new UnaryTransition(States.Playing.ToString(), Input.Pause.ToString(), States.Paused.ToString(), () =>
            {
                return new UnaryTransitionResult("Paused playback");
            }, "Already Playing"));

            stateMachine.Define(new UnaryTransition(States.Playing.ToString(), Input.Stop.ToString(), States.End.ToString(), () =>
            {
                return new UnaryTransitionResult("Stopped");
            }, "Stopped"));

            stateMachine.Define(new BinaryTransition(States.Paused.ToString(), Input.Play.ToString(), States.Playing.ToString(), States.End.ToString(), () =>
            {

                if (canPlay)
                    return new BinaryTransitionResult(BinaryChoice.Left, "Resumed Playback");
                else
                    return new BinaryTransitionResult(BinaryChoice.Right, "Could not Resumed Playback");

            }, "Resume Playback"));

            stateMachine.Define(new UnaryTransition(States.Paused.ToString(), Input.Pause.ToString(), States.Paused.ToString(), () =>
            {
                return new UnaryTransitionResult("Already Paused");
            }, "Already Paused"));

            stateMachine.Define(new UnaryTransition(States.Paused.ToString(), Input.Stop.ToString(), States.End.ToString(), () =>
            {
                return new UnaryTransitionResult("Stopped");
            }, "Stopped"));


            stateMachine.Define(new InvalidTransition(States.End.ToString(), Input.Play.ToString(), "Cannot play from end state"));
            stateMachine.Define(new InvalidTransition(States.End.ToString(), Input.Pause.ToString(), "Cannot pause from end state"));
            stateMachine.Define(new InvalidTransition(States.End.ToString(), Input.Stop.ToString(), "Cannot stop from end state"));

        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(States.Begin.ToString(), stateMachine.State, "Initial state should be 'Begin'");
        }

        [Test]
        public void TransitionEvent()
        {
            stateMachine.OnTransition += (s, e) =>
            {
                Assert.AreEqual(States.Begin.ToString(), e.OldState, "Should have been in the 'Begin' state");
                Assert.AreEqual(Input.Play.ToString(), e.Input, "Action should have been 'Play'");
                Assert.AreEqual(States.Playing.ToString(), e.NewState, "Should have been in the 'Begin' state");
                Assert.AreEqual(States.Playing.ToString(), stateMachine.State, "StateMachine state should be updated");
            };

            stateMachine.Apply(Input.Play.ToString());
        }

        [Test]
        public void ValidUnaryTransition()
        {
            Assert.AreEqual(States.Begin.ToString(), stateMachine.State, "Initial state should be 'Begin'");
            stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), stateMachine.State, "should have moved into 'Playing' state");
        }

        [Test]
        public void DuplicateUnaryTransition()
        {
            Assert.Throws<DuplicateTransitionException>(() =>
            {
                stateMachine.Define(new UnaryTransition(States.Begin.ToString(), Input.Pause.ToString(), States.Paused.ToString(), new Func<UnaryTransitionResult>(() =>
                {
                    return new UnaryTransitionResult("Paused");
                }), "Paused before playing"));
            });
        }


        [Test]
        public void MultipleTransitions()
        {
            Assert.AreEqual(States.Begin.ToString(), stateMachine.State, "Initial state should be 'Begin'");
            stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), stateMachine.State, "should have moved into 'Playing' state");
            stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), stateMachine.State, "should have moved into 'Paused' state");
            stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), stateMachine.State, "should have moved into 'Playing' state");
            stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), stateMachine.State, "should have moved into 'Paused' state");
            stateMachine.Apply(Input.Stop.ToString());
            Assert.AreEqual(States.End.ToString(), stateMachine.State, "should have moved into 'End' state");
        }

        [Test]
        public void BinaryTransition()
        {
            Assert.AreEqual(States.Begin.ToString(), stateMachine.State, "Initial state should be 'Begin'");
            stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), stateMachine.State, "should have moved into 'Playing' state");
            stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), stateMachine.State, "should have moved into 'Paused' state");

            canPlay = false;

            stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.End.ToString(), stateMachine.State, "should have moved into 'End' state");
        }

        [Test]
        public void InvalidTransition()
        {
            Assert.AreEqual(States.Begin.ToString(), stateMachine.State, "Initial state should be 'Begin'");
            stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), stateMachine.State, "should have moved into 'Playing' state");
            stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), stateMachine.State, "should have moved into 'Paused' state");

            canPlay = false;

            stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.End.ToString(), stateMachine.State, "should have moved into 'End' state");

            Assert.Throws<InvalidTransitionException>(() =>
            {
                stateMachine.Apply(Input.Play.ToString());
            });

            Assert.AreEqual(States.End.ToString(), stateMachine.State, "should have remained in the 'End' state");
        }

        [Test]
        public void MultithreadedTransitions()
        {
            stateMachine.Apply(Input.Play.ToString());

            List<string> actions = new List<string>();
            stateMachine.OnTransition += (s, e) =>
            {
                actions.Add(e.Input);
            };

            var task1 = new TaskFactory().StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    stateMachine.Apply(Input.Play.ToString());
                }
            });

            var task2 = new TaskFactory().StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    stateMachine.Apply(Input.Pause.ToString());
                }
            });

            task1.Wait();
            task2.Wait();

            Assert.AreEqual(10, actions.Where(a => a == Input.Pause.ToString()).Count());
            Assert.AreEqual(10, actions.Where(a => a == Input.Play.ToString()).Count());
        }
    }
}