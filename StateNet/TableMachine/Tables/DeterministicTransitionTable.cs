using Aptacode.StateNet.TableMachine.Inputs;
using Aptacode.StateNet.TableMachine.States;
using Aptacode.StateNet.TableMachine.Transitions;
using System;
using System.Linq;

namespace Aptacode.StateNet.TableMachine.Tables
{
    public class DeterministicTransitionTable : TransitionTable
    {
        public DeterministicTransitionTable(StateCollection states, InputCollection inputs) : base(states, inputs) { }

        public void Set(State fromState, Input input, string message)
        {
            var transition = new InvalidTransition(fromState, input, message);
            this.Set(transition);
        }

        public void Set(State fromState, Input input, State toState, string message)
        {
            var transition = new Transition(fromState, input, toState, message);
            this.Set(transition);
        }
    }
}