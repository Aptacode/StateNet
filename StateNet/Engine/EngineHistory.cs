using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine
{
    public class EngineHistory
    {
        public readonly List<Transition> TransitionLog = new List<Transition>();
        public State StartState { get; private set; }

        public IEnumerable<Input> Inputs => TransitionLog.Select(transition => transition.Input);

        public IEnumerable<State> States
        {
            get
            {
                if (StartState == null)
                {
                    return new List<State>();
                }

                return new List<State> {StartState}.Concat(TransitionLog.ToArray()
                    .Select(transition => transition.Target));
            }
        }

        public void SetStart(State source)
        {
            StartState = source;
        }

        public void Log(State source, Input input, State target)
        {
            TransitionLog.Add(new Transition(source, input, target));
        }

        public int StateVisitCount(string name)
        {
            return States.Count(state => state.Name == name);
        }

        public int InputAppliedCount(string name)
        {
            return TransitionLog.Count(transition => transition.Input.Name == name);
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
    }
}