using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.TransitionResult;
using Aptacode.StateNet.Transitions;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests
{
    public class StateMachineTests
    {
        public enum Input
        {
            Play,
            Pause,
            Stop
        }

        public enum States
        {
            Begin,
            Playing,
            Paused,
            End
        }

        private bool _canPlay;

        private StateMachine _stateMachine;

        [SetUp]
        public void Setup()
        {
            _canPlay = true;

            _stateMachine = new StateMachine(StateCollection.FromEnum<States>(), InputCollection.FromEnum<Input>(), States.Begin.ToString());

            _stateMachine.Define(new BinaryTransition(States.Begin.ToString(), Input.Play.ToString(),
                States.Playing.ToString(), States.End.ToString(), () =>
                {
                    if (_canPlay)
                    {
                        return new BinaryTransitionResult(BinaryChoice.Left, "Started Playing");
                    }

                    return new BinaryTransitionResult(BinaryChoice.Right, "Could not start playing");
                }, "Start Playing"));

            _stateMachine.Define(new InvalidTransition(States.Begin.ToString(), Input.Pause.ToString(),
                "Must be Playing to Pause"));

            _stateMachine.Define(new UnaryTransition(States.Begin.ToString(), Input.Stop.ToString(),
                States.End.ToString(), () => new UnaryTransitionResult("Stopped"), "Stop before playing"));

            _stateMachine.Define(new UnaryTransition(States.Playing.ToString(), Input.Play.ToString(),
                States.Playing.ToString(), () => new UnaryTransitionResult("Kept playing"), "Already Playing"));

            _stateMachine.Define(new UnaryTransition(States.Playing.ToString(), Input.Pause.ToString(),
                States.Paused.ToString(), () => new UnaryTransitionResult("Paused playback"), "Already Playing"));

            _stateMachine.Define(new UnaryTransition(States.Playing.ToString(), Input.Stop.ToString(),
                States.End.ToString(), () => { return new UnaryTransitionResult("Stopped"); }, "Stopped"));

            _stateMachine.Define(new BinaryTransition(States.Paused.ToString(), Input.Play.ToString(),
                States.Playing.ToString(), States.End.ToString(), () =>
                {
                    if (_canPlay)
                    {
                        return new BinaryTransitionResult(BinaryChoice.Left, "Resumed Playback");
                    }

                    return new BinaryTransitionResult(BinaryChoice.Right, "Could not Resumed Playback");
                }, "Resume Playback"));

            _stateMachine.Define(new UnaryTransition(States.Paused.ToString(), Input.Pause.ToString(),
                States.Paused.ToString(), () => { return new UnaryTransitionResult("Already Paused"); },
                "Already Paused"));

            _stateMachine.Define(new UnaryTransition(States.Paused.ToString(), Input.Stop.ToString(),
                States.End.ToString(), () => { return new UnaryTransitionResult("Stopped"); }, "Stopped"));


            _stateMachine.Define(new InvalidTransition(States.End.ToString(), Input.Play.ToString(),
                "Cannot play from end state"));
            _stateMachine.Define(new InvalidTransition(States.End.ToString(), Input.Pause.ToString(),
                "Cannot pause from end state"));
            _stateMachine.Define(new InvalidTransition(States.End.ToString(), Input.Stop.ToString(),
                "Cannot stop from end state"));
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(States.Begin.ToString(), _stateMachine.State, "Initial state should be 'Begin'");
        }

        [Test]
        public void TransitionEvent()
        {
            _stateMachine.OnTransition += (s, e) =>
            {
                Assert.AreEqual(States.Begin.ToString(), e.OldState, "Should have been in the 'Begin' state");
                Assert.AreEqual(Input.Play.ToString(), e.Input, "Action should have been 'Play'");
                Assert.AreEqual(States.Playing.ToString(), e.NewState, "Should have been in the 'Begin' state");
                Assert.AreEqual(States.Playing.ToString(), _stateMachine.State, "StateMachine state should be updated");
            };

            _stateMachine.Apply(Input.Play.ToString());
        }

        [Test]
        public void ValidUnaryTransition()
        {
            Assert.AreEqual(States.Begin.ToString(), _stateMachine.State, "Initial state should be 'Begin'");
            _stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), _stateMachine.State, "should have moved into 'Playing' state");
        }

        [Test]
        public void DuplicateUnaryTransition()
        {
            Assert.Throws<DuplicateTransitionException>(() =>
            {
                _stateMachine.Define(new UnaryTransition(States.Begin.ToString(), Input.Pause.ToString(),
                    States.Paused.ToString(), () => { return new UnaryTransitionResult("Paused"); },
                    "Paused before playing"));
            });
        }


        [Test]
        public void MultipleTransitions()
        {
            Assert.AreEqual(States.Begin.ToString(), _stateMachine.State, "Initial state should be 'Begin'");
            _stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), _stateMachine.State, "should have moved into 'Playing' state");
            _stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), _stateMachine.State, "should have moved into 'Paused' state");
            _stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), _stateMachine.State, "should have moved into 'Playing' state");
            _stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), _stateMachine.State, "should have moved into 'Paused' state");
            _stateMachine.Apply(Input.Stop.ToString());
            Assert.AreEqual(States.End.ToString(), _stateMachine.State, "should have moved into 'End' state");
        }

        [Test]
        public void BinaryTransition()
        {
            Assert.AreEqual(States.Begin.ToString(), _stateMachine.State, "Initial state should be 'Begin'");
            _stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), _stateMachine.State, "should have moved into 'Playing' state");
            _stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), _stateMachine.State, "should have moved into 'Paused' state");

            _canPlay = false;

            _stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.End.ToString(), _stateMachine.State, "should have moved into 'End' state");
        }

        [Test]
        public void InvalidTransition()
        {
            Assert.AreEqual(States.Begin.ToString(), _stateMachine.State, "Initial state should be 'Begin'");
            _stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.Playing.ToString(), _stateMachine.State, "should have moved into 'Playing' state");
            _stateMachine.Apply(Input.Pause.ToString());
            Assert.AreEqual(States.Paused.ToString(), _stateMachine.State, "should have moved into 'Paused' state");

            _canPlay = false;

            _stateMachine.Apply(Input.Play.ToString());
            Assert.AreEqual(States.End.ToString(), _stateMachine.State, "should have moved into 'End' state");

            Assert.Throws<InvalidTransitionException>(() => { _stateMachine.Apply(Input.Play.ToString()); });

            Assert.AreEqual(States.End.ToString(), _stateMachine.State, "should have remained in the 'End' state");
        }

        [Test]
        public void MultithreadedTransitions()
        {
            _stateMachine.Apply(Input.Play.ToString());

            var actions = new List<string>();
            _stateMachine.OnTransition += (s, e) => { actions.Add(e.Input); };

            var task1 = new TaskFactory().StartNew(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    _stateMachine.Apply(Input.Play.ToString());
                }
            });

            var task2 = new TaskFactory().StartNew(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    _stateMachine.Apply(Input.Pause.ToString());
                }
            });

            task1.Wait();
            task2.Wait();

            Assert.AreEqual(10, actions.Where(a => a == Input.Pause.ToString()).Count());
            Assert.AreEqual(10, actions.Where(a => a == Input.Play.ToString()).Count());
        }
    }
}