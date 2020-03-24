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
                    .Select(transition => transition.Destination));
            }
        }

        public void SetStart(State source)
        {
            StartState = source;
        }

        public void Log(State source, Input input, State destination)
        {
            TransitionLog.Add(new Transition(source, input, destination));
        }

        public int StateVisitCount(string name)
        {
            return States.Count(state => state.Name == name);
        }

        public int InputAppliedCount(string name)
        {
            return TransitionLog.Count(transition => transition.Input.Name == name);
        }

        public int TransitionInCount(string inputName, string stateName)
        {
            return TransitionLog.Count(transition =>
                transition.Input.Name == inputName &&
                transition.Destination.Name == stateName);
        }

        public int TransitionOutCount(string stateName, string inputName)
        {
            return TransitionLog.Count(transition =>
                transition.Source.Name == stateName &&
                transition.Input.Name == inputName);
        }
    }
}