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

        public void Set(TStates fromState, TInputs input, string message)
        {
            var transition = new InvalidTransition(fromState.ToString(), input.ToString(), message);
            this.Set(transition);
        }

        public void Set(TStates fromState, TInputs input, TStates toState, string message)
        {
            var transition = new Transition<Tuple<string>>(fromState.ToString(),
                                                           input.ToString(),
                                                           new Tuple<string>(toState.ToString()),
                                                           (states) => states.Item1,
                                                           message);
            this.Set(transition);
        }

        public void Set(TStates fromState,
                        TInputs input,
                        TStates toState1,
                        TStates toState2,
                        Func<Tuple<string, string>, string> choiceFunction,
                        string message)
        {
            var transition = new Transition<Tuple<string, string>>(fromState.ToString(),
                                                                   input.ToString(),
                                                                   new Tuple<string, string>(toState1.ToString(),
                                                                                             toState2.ToString()),
                                                                   choiceFunction,
                                                                   message);
            this.Set(transition);
        }

        public void Set(TStates fromState,
                        TInputs input,
                        TStates toState1,
                        TStates toState2,
                        TStates toState3,
                        Func<Tuple<string, string, string>, string> choiceFunction,
                        string message)
        {
            var transition = new Transition<Tuple<string, string, string>>(fromState.ToString(),
                                                                           input.ToString(),
                                                                           new Tuple<string, string, string>(toState1.ToString(),
                                                                                                             toState2.ToString(),
                                                                                                             toState3.ToString()),
                                                                           choiceFunction,
                                                                           message);
            this.Set(transition);
        }

        public void Set(TStates fromState,
                        TInputs input,
                        TStates toState1,
                        TStates toState2,
                        TStates toState3,
                        TStates toState4,
                        Func<Tuple<string, string, string, string>, string> choiceFunction,
                        string message)
        {
            var transition = new Transition<Tuple<string, string, string, string>>(fromState.ToString(),
                                                                                   input.ToString(),
                                                                                   new Tuple<string, string, string, string>(toState1.ToString(),
                                                                                                                             toState2.ToString(),
                                                                                                                             toState3.ToString(),
                                                                                                                             toState4.ToString()),
                                                                                   choiceFunction,
                                                                                   message);
            this.Set(transition);
        }
    }
}