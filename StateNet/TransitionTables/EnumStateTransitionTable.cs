using Aptacode.StateNet.TransitionResults;
using Aptacode.StateNet.Transitions;
using System;
using System.Linq;

namespace Aptacode.StateNet.TransitionTables
{
    public class EnumStateTransitionTable<TStates, TInputs> : StateTransitionTable
        where TStates : System.Enum
        where TInputs : System.Enum
    {
        public EnumStateTransitionTable() : base(StateCollection.FromEnum<TStates>(),
                                                 InputCollection.FromEnum<TInputs>())
        { }

        public void Set(TStates fromState, TInputs input, string message = "Invalid")
        {
            var transition = new InvalidTransition(fromState.ToString(), input.ToString(), message);
            this.Set(transition);
        }

        public void Set(TStates fromState, TInputs input, TStates toState, string message = "Unary")
        {
            var transition = new UnaryTransition(fromState.ToString(), input.ToString(), toState.ToString(), message);
            this.Set(transition);
        }

        public void Set(TStates fromState,
                        TInputs input,
                        Func<int> transitionChoice,
                        string message = "Nary",
                        params TStates[] toStates)
        {
            var transition = new NaryTransition(fromState.ToString(),
                                                input.ToString(),
                                                toStates.Select(state => state.ToString()).ToList(),
                                                transitionChoice,
                                                message);
            this.Set(transition);
        }

        public void Set(TStates fromState,
                        TInputs input,
                        TStates toState1,
                        TStates toState2,
                        Func<BinaryChoice> transitionChoice,
                        string message = "Binary")
        {
            var transition = new BinaryTransition(fromState.ToString(),
                                                  input.ToString(),
                                                  toState1.ToString(),
                                                  toState2.ToString(),
                                                  transitionChoice,
                                                  message);
            this.Set(transition);
        }
    }
}