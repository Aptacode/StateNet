using Aptacode.StateNet.Inputs;
using Aptacode.StateNet.States;
using Aptacode.StateNet.Transitions;
using NUnit.Framework;
using System;

namespace Aptacode.StateNet.Tests
{
    public class StateTransitionTableTests
    {
        private EnumInputCollection<Inputs> _inputCollection;
        private EnumStateCollection<States> _stateCollection;
        private TransitionTable _stateTransitionTable;


        [Test]
        public void ClearTransition()
        {
            _stateTransitionTable.Set(_stateCollection[States.Playing],
                                      _inputCollection[Inputs.Pause],
                                      _stateCollection[States.Paused],
                                      "Pause Playback");

            var transition = _stateTransitionTable.Get(_stateCollection[States.Playing], _inputCollection[Inputs.Pause]);
            _stateTransitionTable.Clear(transition);
            Assert.That(_stateTransitionTable.Get(_stateCollection[States.Playing], _inputCollection[Inputs.Pause]) is InvalidTransition);
        }

        [Test]
        public void InitializeToNull()
        {
            foreach(var state in (States[]) Enum.GetValues(typeof(States)))
            {
                foreach(var action in (Inputs[])Enum.GetValues(typeof(Inputs)))
                {
                    Assert.That(_stateTransitionTable.Get(_stateCollection[state], _inputCollection[action]) is InvalidTransition);
                }
            }
        }

        [Test]
        public void OverwriteTransition()
        {
            _stateTransitionTable.Set(_stateCollection[States.Playing],
                                      _inputCollection[Inputs.Pause],
                                      _stateCollection[States.Paused],
                                      "Pause Playback");
            _stateTransitionTable.Set(_stateCollection[States.Playing],
                                      _inputCollection[Inputs.Pause],
                                      _stateCollection[States.End],
                                      "Stop Playback");

            var transition = _stateTransitionTable.Get(_stateCollection[States.Playing], _inputCollection[Inputs.Pause]);

            Assert.AreEqual(_stateCollection[States.End], transition.Apply());
        }

        [Test]
        public void SetTransition()
        {
            _stateTransitionTable.Set(_stateCollection[States.Playing],
                                      _inputCollection[Inputs.Pause],
                                      _stateCollection[States.Paused],
                                      "Pause Playback");
            var transition = _stateTransitionTable.Get(_stateCollection[States.Playing], _inputCollection[Inputs.Pause]);
            Assert.NotNull(transition);
        }

        [SetUp]
        public void Setup()
        {
            _inputCollection = new EnumInputCollection<Inputs>();
            _stateCollection = new EnumStateCollection<States>();

            _stateTransitionTable = new TransitionTable(_stateCollection, _inputCollection);
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