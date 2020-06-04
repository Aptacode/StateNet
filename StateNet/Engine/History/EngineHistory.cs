using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.History
{
    public class EngineHistory : IEngineHistory
    {
        private readonly List<Transition> _transitionLog = new List<Transition>();
        public IEnumerable<Transition> TransitionLog => _transitionLog;

        public void Log(State source, Input input, State target)
        {
            if (StartState == null) StartState = source;

            _transitionLog.Add(new Transition(source, input, target));
        }

        public IEnumerable<Transition> GetLastTransitionsOut(string state, int count)
        {
            return TakeLast(TransitionLog.Where(t => t.Source == state), count);
        }

        public IEnumerable<Transition> LastTransitions(int count)
        {
            return TakeLast(TransitionLog, count);
        }

        public int TransitionInCount(string input, string state)
        {
            return TransitionLog.Count(transition =>
                transition.Input.Name == input &&
                transition.Target.Name == state);
        }

        public int TransitionOutCount(string state, string input)
        {
            return TransitionLog.Count(transition =>
                transition.Source.Name == state &&
                transition.Input.Name == input);
        }

        public static IEnumerable<T> TakeLast<T>(IEnumerable<T> source, int count)
        {
            var sourceList = source.ToList();
            return sourceList.Skip(Math.Max(0, sourceList.Count - count));
        }

        #region Inputs

        public IEnumerable<Input> Inputs => TransitionLog.Select(transition => transition.Input);

        public int InputAppliedCount(string name)
        {
            return TransitionLog.Count(transition => transition.Input.Name == name);
        }

        #endregion

        #region States

        public State StartState { get; private set; }

        public IEnumerable<State> States
        {
            get
            {
                if (StartState == null) return new List<State>();

                if (!TransitionLog.Any()) return new List<State> {StartState};


                return new List<State> {StartState}.Concat(TransitionLog.ToArray()
                    .Select(transition => transition.Target));
            }
        }

        public void SetStart(State source)
        {
            StartState = source;
        }

        public int StateVisitCount(string name)
        {
            return States.Count(state => state.Name == name);
        }

        #endregion
    }
}