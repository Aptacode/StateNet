using Aptacode.StateNet.FiniteStateMachine.Inputs;
using Aptacode.StateNet.FiniteStateMachine.States;
using System;

namespace Aptacode.StateNet.FiniteStateMachine.Transitions
{
    public class Transition : BaseTransition
    {
        /// <summary>
        /// A transition which CAN NOT exist
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="message"></param>
        public Transition(State state, Input input, State destination, string message) : base(state, input, message) => Destination =
            destination;

        /// <summary>
        /// Throws an exception as an invalidTransition cannot be applied.
        /// </summary>
        /// <returns></returns>
        public override State Apply() => Destination;

        public override string ToString() => $"Unary Transition: {Origin}({Input})->{{ {Destination} }}";

        public State Destination { get; }
    }
}
