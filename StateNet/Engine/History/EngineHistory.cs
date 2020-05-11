using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.Engine.History
{
    public class EngineHistory : IEngineHistory
    {
        public List<Transition> TransitionLog { get; set; } = new List<Transition>();

        public void Log(State source, Input input, State target)
        {
            if (StartState == null)
            {
                StartState = source;
            }

            TransitionLog.Add(new Transition(source, input, target));
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
                if (StartState == null)
                {
                    return new List<State>();
                }

                if (!TransitionLog.Any())
                {
                    return new List<State> {StartState};
                }


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