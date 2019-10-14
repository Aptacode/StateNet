using System;
using Aptacode.StateNet.StateTransitionTable;
using Aptacode.StateNet.TransitionResult;
using Aptacode.StateNet.Transitions;
using NUnit.Framework;

namespace Aptacode.StateNet.Tests.StateTransitionTable
{
    public class StateTransitionTableTests
    {
        public enum States { Begin, Playing, Paused, End };
        public enum Actions { Play, Pause, Stop };

        private StateCollection _stateCollection;
        private InputCollection _inputCollection;
        private IStateTransitionTable _stateTransitionTable;

        [SetUp]
        public void Setup()
        {
            _stateCollection = StateCollection.FromEnum<States>();
            _inputCollection = InputCollection.FromEnum<Actions>();
            _stateTransitionTable = new DictionaryStateTransitionTable();
            _stateTransitionTable.Setup(_stateCollection, _inputCollection);
        }

        [Test]
        public void InitialiseToNull()
        {
            
            foreach (States state in (States[])Enum.GetValues(typeof(States)))
            {
                foreach (Actions action in (Actions[])Enum.GetValues(typeof(Actions)))
                {
                    Assert.IsNull(_stateTransitionTable.Get(state.ToString(), action.ToString()));
                }
            }
        }

        [Test]
        public void SetTransition()
        {
            _stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), () => new UnaryTransitionResult("Paused"), "Pause"));
            var transition = _stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            Assert.NotNull(transition);
        }


        [Test]
        public void ClearTransition()
        {
            _stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), () => new UnaryTransitionResult("Paused"), "Pause"));
            var transition = _stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            _stateTransitionTable.Clear(transition);
            Assert.IsNull(_stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString()));
        }

        [Test]
        public void OverwriteTransition()
        {
            _stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), () => new UnaryTransitionResult("Paused"), "Pause"));
            _stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.End.ToString(), () => new UnaryTransitionResult("Ended"), "Pause"));
            var transition = _stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            _stateTransitionTable.Clear(transition);
            Assert.AreEqual(States.End.ToString(), transition.Apply());
        }
    }
}