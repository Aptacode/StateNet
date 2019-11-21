using Aptacode.StateNet.Transitions;
using Aptacode.StateNet.TransitionTables;
using NUnit.Framework;
using System;

namespace Aptacode.StateNet.Tests.StateTransitionTable
{
    public class StateTransitionTableTests
    {
        private StringStateTransitionTable _stateTransitionTable;


        [Test]
        public void ClearTransition()
        {
            _stateTransitionTable.Set(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), "Pause Playback");
            var transition = _stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            _stateTransitionTable.Clear(transition);
            Assert.That(_stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString()) is InvalidTransition);
        }

        [Test]
        public void InitializeToNull()
        {
            foreach(var state in (States[]) Enum.GetValues(typeof(States)))
            {
                foreach(var action in (Actions[])Enum.GetValues(typeof(Actions)))
                {
                    Assert.That(_stateTransitionTable.Get(state.ToString(), action.ToString()) is InvalidTransition);
                }
            }
        }

        [Test]
        public void OverwriteTransition()
        {
            _stateTransitionTable.Set(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), "Pause Playback");
            _stateTransitionTable.Set(States.Playing.ToString(), Actions.Pause.ToString(), States.End.ToString(), "Stop Playback");

            var transition = _stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            _stateTransitionTable.Clear(transition);
            Assert.AreEqual(States.End.ToString(), transition.Apply());
        }

        [Test]
        public void SetTransition()
        {
            _stateTransitionTable.Set(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), "Pause Playback");
            var transition = _stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            Assert.NotNull(transition);
        }

        [SetUp]
        public void Setup()
        {
            var stateCollection = StateCollection.FromEnum<States>();
            var inputCollection = InputCollection.FromEnum<Actions>();
            _stateTransitionTable = new StringStateTransitionTable(stateCollection, inputCollection);
        }

        public enum Actions
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