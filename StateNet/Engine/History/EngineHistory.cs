using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.History
{
    public class EngineHistory
    {
        private readonly List<Transition> _transitionLog = new List<Transition>();
        public State StartState { get; private set; }

        public IEnumerable<Input> Inputs => _transitionLog.Select(transition => transition.Input);

        public IEnumerable<State> States
        {
            get
            {
                if (StartState == null)
                {
                    return new List<State>();
                }

                return new List<State> {StartState}.Concat(_transitionLog.ToArray()
                    .Select(transition => transition.Target));
            }
        }

        public void SetStart(State source)
        {
            StartState = source;
        }

        public void Log(State source, Input input, State target)
        {
            _transitionLog.Add(new Transition(source, input, target));
        }

        public int StateVisitCount(string name)
        {
            return States.Count(state => state.Name == name);
        }

        public int InputAppliedCount(string name)
        {
            return _transitionLog.Count(transition => transition.Input.Name == name);
        }

        public int TransitionInCount(string input, string state)
        {
            return _transitionLog.Count(transition =>
                transition.Input.Name == input &&
                transition.Target.Name == state);
        }

        public int TransitionOutCount(string state, string input)
        {
            return _transitionLog.Count(transition =>
                transition.Source.Name == state &&
                transition.Input.Name == input);
        }
    }
}