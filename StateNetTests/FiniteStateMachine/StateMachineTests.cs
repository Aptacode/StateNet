using Aptacode.StateNet.TableMachine;
using Aptacode.StateNet.TableMachine.Inputs;
using Aptacode.StateNet.TableMachine.States;
using Aptacode.StateNet.TableMachine.Tables;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aptacode.StateNet.Tests.FiniteStateMachine
{
    public class StateMachineTests
    {
        private bool _canPlay;

        private EnumInputCollection<Inputs> _inputCollection;
        private EnumStateCollection<States> _stateCollection;
        private TableEngine _stateMachine;
        private NonDeterministicTransitionTable _stateTransitionTable;

        [Test]
        public void BinaryTransition()
        {
            var actualStateLog = new List<State>();
            var expectedStateLog = new List<State> { new State("Playing"), new State("Paused"), new State("End") };

            _stateMachine.OnTransition += (s, e) =>
            {
                actualStateLog.Add(e.NewState);
            };

            _stateMachine.Apply(_inputCollection[Inputs.Play]);
            _stateMachine.Apply(_inputCollection[Inputs.Pause]);
            _stateMachine.Apply(_inputCollection[Inputs.Stop]);

            Assert.That(() => actualStateLog,
                        Is.EquivalentTo(expectedStateLog).After(100).MilliSeconds.PollEvery(1).MilliSeconds);
        }


        [Test]
        public void InitialState() => Assert.That(() => _stateCollection[States.Begin],
                                                  Is.EqualTo(_stateMachine.State).After(100).MilliSeconds.PollEvery(1)
            .MilliSeconds,
                                                  "Initial state should be 'Begin'");


        [Test]
        public void InvalidTransition()
        {
            var expectedInvalidState = _stateCollection[States.End];
            var expectedInvalidInput = _inputCollection[Inputs.Play];

            var actualInvalidState = _stateCollection[States.Begin];
            var actualInvalidInput = _inputCollection[Inputs.Play];

            _stateMachine.OnInvalidTransition += (s, e) =>
            {
                actualInvalidState = e.OldState;
                actualInvalidInput = e.Input;
            };

            _stateMachine.Apply(_inputCollection[Inputs.Play]);
            _stateMachine.Apply(_inputCollection[Inputs.Stop]);
            _stateMachine.Apply(_inputCollection[Inputs.Play]);

            Assert.That(() => (actualInvalidState, actualInvalidInput),
                        Is.EqualTo((expectedInvalidState, expectedInvalidInput)).After(100).MilliSeconds.PollEvery(1)
                .MilliSeconds);
        }

        [Test]
        public void MultithreadedTransitions()
        {
            var playCount = 0;
            var pauseCount = 0;
            _stateMachine.OnTransition += (s, e) =>
            {
                if(e.Input.Name == "Play")
                {
                    playCount++;
                } else if(e.Input.Name == "Pause")
                {
                    pauseCount++;
                }
            };

            var task1 = new TaskFactory().StartNew(() =>
            {
                for(var i = 0; i < 10; i++)
                {
                    _stateMachine.Apply(_inputCollection[Inputs.Play]);
                }
            });

            var task2 = new TaskFactory().StartNew(() =>
            {
                for(var i = 0; i < 10; i++)
                {
                    _stateMachine.Apply(_inputCollection[Inputs.Pause]);
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
            _inputCollection = new EnumInputCollection<Inputs>();
            _stateCollection = new EnumStateCollection<States>();
            _stateTransitionTable = new NonDeterministicTransitionTable(_stateCollection, _inputCollection);

            _stateTransitionTable.Set(_stateCollection[States.Begin],
                                      _inputCollection[Inputs.Play],
                                      _stateCollection[States.Playing],
                                      _stateCollection[States.End],

                                      (states) =>
            {
                if(_canPlay)
                {
                    return states.Item1;
                }

                return states.Item2;
            },
                                      string.Empty);

            _stateTransitionTable.Set(_stateCollection[States.Begin], _inputCollection[Inputs.Pause], string.Empty);
            _stateTransitionTable.Set(_stateCollection[States.Begin],
                                      _inputCollection[Inputs.Stop],
                                      _stateCollection[States.End],
                                      string.Empty);

            _stateTransitionTable.Set(_stateCollection[States.Playing],
                                      _inputCollection[Inputs.Play],
                                      _stateCollection[States.Playing],
                                      string.Empty);
            _stateTransitionTable.Set(_stateCollection[States.Playing],
                                      _inputCollection[Inputs.Pause],
                                      _stateCollection[States.Paused],
                                      string.Empty);
            _stateTransitionTable.Set(_stateCollection[States.Playing],
                                      _inputCollection[Inputs.Stop],
                                      _stateCollection[States.End],
                                      string.Empty);

            _stateTransitionTable.Set(_stateCollection[States.Paused],
                                      _inputCollection[Inputs.Play],
                                      _stateCollection[States.Playing],
                                      _stateCollection[States.End],
                                      (states) =>
            {
                if(_canPlay)
                {
                    return states.Item1;
                }

                return states.Item2;
            },
                                      string.Empty);


            _stateTransitionTable.Set(_stateCollection[States.Paused],
                                      _inputCollection[Inputs.Pause],
                                      _stateCollection[States.Paused],
                                      string.Empty);
            _stateTransitionTable.Set(_stateCollection[States.Paused],
                                      _inputCollection[Inputs.Stop],
                                      _stateCollection[States.End],
                                      string.Empty);

            _stateTransitionTable.Set(_stateCollection[States.End], _inputCollection[Inputs.Play], string.Empty);
            _stateTransitionTable.Set(_stateCollection[States.End], _inputCollection[Inputs.Pause], string.Empty);
            _stateTransitionTable.Set(_stateCollection[States.End], _inputCollection[Inputs.Stop], string.Empty);

            _stateMachine = new TableEngine(_stateTransitionTable);

            _stateMachine.Start(_stateCollection[States.Begin]);
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

            _stateMachine.Apply(_inputCollection[Inputs.Play]);
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