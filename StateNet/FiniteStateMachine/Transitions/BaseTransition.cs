using Aptacode.StateNet.FiniteStateMachine.Inputs;
using Aptacode.StateNet.FiniteStateMachine.States;
using System;
using System.Linq;

namespace Aptacode.StateNet.FiniteStateMachine.Transitions
{
    public abstract class BaseTransition
    {
        protected BaseTransition(State origin, Input input, string message)
        {
            Origin = origin;
            Input = input;
            Message = message;
        }

        public abstract State Apply();

        public abstract override string ToString();

        /// <summary>
        /// The Input which when applied to the 'State' causes the transition
        /// </summary>
        public Input Input { get; }

        /// <summary>
        /// A message describing the transition
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The starting state which the transition is relating to
        /// </summary>
        public State Origin { get; }
    }
}