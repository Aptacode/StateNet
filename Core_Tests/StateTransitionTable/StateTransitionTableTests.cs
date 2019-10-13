using Aptacode.StateNet.Core.StateTransitionTable;
using Aptacode.StateNet.Core.Transitions;
using NUnit.Framework;
using System;

namespace Aptacode.StateNet.Core_Tests
{
    public class StateTransitionTableTests
    {
        public enum States { Begin, Playing, Paused, End };
        public enum Actions { Play, Pause, Stop };
        private static object[] _sourceLists = {
            new object[] {new ArrayStateTransitionTable<States,Actions>()},
            new object[] {new DictionaryStateTransitionTable<States,Actions>()}
            };

        [SetUp]
        public void Setup()
        {

        }

        [Test, TestCaseSource("_sourceLists")]
        public void InitialiseToNull(IStateTransitionTable<States, Actions> stateTransitionTable)
        {
            foreach (States state in (States[])Enum.GetValues(typeof(States)))
            {
                foreach (Actions action in (Actions[])Enum.GetValues(typeof(Actions)))
                {
                    Assert.IsNull(stateTransitionTable.Get(state, action));
                }
            }
        }

        [Test, TestCaseSource("_sourceLists")]
        public void SetTransition(IStateTransitionTable<States, Actions> stateTransitionTable)
        {
            stateTransitionTable.Set(new UnaryTransition<States, Actions>(States.Playing, Actions.Pause, States.Paused, () => { return new Core.TransitionResult.UnaryTransitionResult("Paused"); }, "Pause"));
            var Transition = stateTransitionTable.Get(States.Playing, Actions.Pause);
            Assert.NotNull(Transition);
        }

        [Test, TestCaseSource("_sourceLists")]
        public void ClearTransition(IStateTransitionTable<States, Actions> stateTransitionTable)
        {
            stateTransitionTable.Set(new UnaryTransition<States, Actions>(States.Playing, Actions.Pause, States.Paused, () => { return new Core.TransitionResult.UnaryTransitionResult("Paused"); }, "Pause"));
            var Transition = stateTransitionTable.Get(States.Playing, Actions.Pause);
            stateTransitionTable.Clear(Transition);
            Assert.IsNull(stateTransitionTable.Get(States.Playing, Actions.Pause));
        }

        [Test, TestCaseSource("_sourceLists")]
        public void OverwriteTransition(IStateTransitionTable<States, Actions> stateTransitionTable)
        {
            stateTransitionTable.Set(new UnaryTransition<States, Actions>(States.Playing, Actions.Pause, States.Paused, () => { return new Core.TransitionResult.UnaryTransitionResult("Paused"); }, "Pause"));
            stateTransitionTable.Set(new UnaryTransition<States, Actions>(States.Playing, Actions.Pause, States.End, () => { return new Core.TransitionResult.UnaryTransitionResult("Ended"); }, "Pause"));
            var Transition = stateTransitionTable.Get(States.Playing, Actions.Pause);
            stateTransitionTable.Clear(Transition);
            Assert.AreEqual(States.End, Transition.Apply());
        }
    }
}