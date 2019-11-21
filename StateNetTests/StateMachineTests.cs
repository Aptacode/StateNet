using Aptacode.StateNet.TransitionResults;
using Aptacode.StateNet.TransitionTables;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.StateNet.Tests
{
    public class StateMachineTests
    {
        private bool _canPlay;

        private StateMachine _stateMachine;

        [Test]
        public void BinaryTransition()
        {
            var actualStateLog = new List<string>();
            var expectedStateLog = new List<string> { "Playing", "Paused", "End" };

            _stateMachine.OnTransition += (s, e) =>
            {
                actualStateLog.Add(e.NewState);
            };

            _stateMachine.Apply(Inputs.Play.ToString());
            _stateMachine.Apply(Inputs.Pause.ToString());
            _stateMachine.Apply(Inputs.Stop.ToString());

            Assert.That(() => actualStateLog,
                        Is.EquivalentTo(expectedStateLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }


        [Test]
        public void InitialState() => Assert.AreEqual(States.Begin.ToString(),
                                                      _stateMachine.State,
                                                      "Initial state should be 'Begin'") ;

        [Test]
        public void InvalidTransition()
        {
            var expectedInvalidState = string.Empty;
            var expectedInvalidInput = string.Empty;

            var actualInvalidState = string.Empty;
            var actualInvalidInput = string.Empty;
            _stateMachine.OnInvalidTransition += (s, e) =>
            {
                actualInvalidState = e.State;
                actualInvalidInput = e.Input;
            };

            _stateMachine.Apply(Inputs.Play.ToString());
            _stateMachine.Apply(Inputs.Stop.ToString());
            _stateMachine.Apply(Inputs.Play.ToString());

            Assert.That(() => (expectedInvalidState, expectedInvalidInput),
                        Is.EqualTo((actualInvalidState, actualInvalidInput)).After(100).MilliSeconds.PollEvery(1)
                .MilliSeconds);
        }

        [Test]
        public void MultithreadedTransitions()
        {
            var actions = new List<string>();

            var playCount = 0;
            var pauseCount = 0;
            _stateMachine.OnTransition += (s, e) =>
            {
                if(e.Input == "Play")
                {
                    playCount++;
                } else if(e.Input == "Pause")
                {
                    pauseCount++;
                }
            };

            var task1 = new TaskFactory().StartNew(async() =>
            {
                for(var i = 0; i < 10; i++)
                {
                    _stateMachine.Apply(Inputs.Play.ToString());
                }
            });

            var task2 = new TaskFactory().StartNew(async() =>
            {
                for(var i = 0; i < 10; i++)
                {
                    _stateMachine.Apply(Inputs.Pause.ToString());
                }
            });

            task1.Wait();
            task2.Wait();


            Assert.That(() => (10 == playCount) && (10 == pauseCount),
                        Is.True.After(1000).MilliSeconds.PollEvery(1).MilliSeconds);
        }

        [SetUp]
        public void Setup()
        {
            _canPlay = true;
            var enumTransitionTable = new EnumStateTransitionTable<States, Inputs>();

            enumTransitionTable.Set(States.Begin,
                                    Inputs.Play,
                                    States.Playing,
                                    States.End,
                                    () =>
            {
                if(_canPlay)
                {
                    return BinaryChoice.Left;
                }

                return BinaryChoice.Right;
            });

            enumTransitionTable.Set(States.Begin, Inputs.Pause);
            enumTransitionTable.Set(States.Begin, Inputs.Stop, States.End);

            enumTransitionTable.Set(States.Playing, Inputs.Play, States.Playing);
            enumTransitionTable.Set(States.Playing, Inputs.Pause, States.Paused);
            enumTransitionTable.Set(States.Playing, Inputs.Stop, States.End);

            enumTransitionTable.Set(States.Paused,
                                    Inputs.Play,
                                    States.Playing,
                                    States.End,
                                    () =>
            {
                if(_canPlay)
                {
                    return BinaryChoice.Left;
                }

                return BinaryChoice.Right;
            });


            enumTransitionTable.Set(States.Paused, Inputs.Pause, States.Paused);
            enumTransitionTable.Set(States.Paused, Inputs.Stop, States.End);

            enumTransitionTable.Set(States.End, Inputs.Play);
            enumTransitionTable.Set(States.End, Inputs.Pause);
            enumTransitionTable.Set(States.End, Inputs.Stop);

            _stateMachine = new StateMachine(enumTransitionTable, States.Begin.ToString());
            _stateMachine.Start();
        }

        [Test]
        public void TransitionEvent()
        {
            _stateMachine.OnTransition += (s, e) =>
            {
                Assert.AreEqual(States.Begin.ToString(), e.OldState, "Should have been in the 'Begin' state");
                Assert.AreEqual(Inputs.Play.ToString(), e.Input, "Action should have been 'Play'");
                Assert.AreEqual(States.Playing.ToString(), e.NewState, "Should have been in the 'Begin' state");
                Assert.AreEqual(States.Playing.ToString(), _stateMachine.State, "StateMachine state should be updated");
            };

            _stateMachine.Apply(Inputs.Play.ToString());
        }

        public enum Inputs
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
    }
}