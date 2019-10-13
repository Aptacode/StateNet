using Aptacode.StateNet.Core;
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

            StateCollection StateCollection;
            InputCollection InputCollection;
        IStateTransitionTable stateTransitionTable;
        [SetUp]
        public void Setup()
        {
            StateCollection = StateCollection.FromEnum<States>();
            InputCollection = InputCollection.FromEnum<Actions>();
            stateTransitionTable = new DictionaryStateTransitionTable();
            stateTransitionTable.Setup(StateCollection, InputCollection);
        }

        [Test]
        public void InitialiseToNull()
        {
            
            foreach (States state in (States[])Enum.GetValues(typeof(States)))
            {
                foreach (Actions action in (Actions[])Enum.GetValues(typeof(Actions)))
                {
                    Assert.IsNull(stateTransitionTable.Get(state.ToString(), action.ToString()));
                }
            }
        }

        [Test]
        public void SetTransition()
        {
            stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), () => { return new Core.TransitionResult.UnaryTransitionResult("Paused"); }, "Pause"));
            var Transition = stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            Assert.NotNull(Transition);
        }


        [Test]
        public void ClearTransition()
        {
            stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), () => { return new Core.TransitionResult.UnaryTransitionResult("Paused"); }, "Pause"));
            var Transition = stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            stateTransitionTable.Clear(Transition);
            Assert.IsNull(stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString()));
        }

        [Test]
        public void OverwriteTransition()
        {
            stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.Paused.ToString(), () => { return new Core.TransitionResult.UnaryTransitionResult("Paused"); }, "Pause"));
            stateTransitionTable.Set(new UnaryTransition(States.Playing.ToString(), Actions.Pause.ToString(), States.End.ToString(), () => { return new Core.TransitionResult.UnaryTransitionResult("Ended"); }, "Pause"));
            var Transition = stateTransitionTable.Get(States.Playing.ToString(), Actions.Pause.ToString());
            stateTransitionTable.Clear(Transition);
            Assert.AreEqual(States.End.ToString(), Transition.Apply());
        }
    }
}