using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.TransitionResult;
using System;

namespace Aptacode.StateNet.Transitions
{
    public class BinaryTransition : ValidTransition
    {
        /// <summary>
        /// Defines a transition to either the 'Left' or 'Right' State when the 'input' is applied to the current state 
        ///    The choice of 'Left' or 'Right' is determined by the result of invoking the user defined
        /// AcceptanceCallback
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="leftState"></param>
        /// <param name="rightState"></param>
        /// <param name="acceptanceCallback"></param>
        /// <param name="message"></param>
        public BinaryTransition(string state,
                                string input,
                                string leftState,
                                string rightState,
                                Func<BinaryTransitionResult> acceptanceCallback,
                                string message) : base(state, input, message)
        {
            LeftState = leftState;
            RightState = rightState;
            AcceptanceCallback = acceptanceCallback;
        }

        public string LeftState { get; }

        public string RightState { get; }

        protected Func<BinaryTransitionResult> AcceptanceCallback { get; set; }

        public override string Apply()
        {
            if(AcceptanceCallback?.Invoke()?.Success != true)
            {
                throw new AcceptanceCallbackFailedException(State, Input);
            }

            if((AcceptanceCallback?.Invoke()).Choice == BinaryChoice.Left)
            {
                return LeftState;
            }

            return RightState;
        }

        public override string ToString() => $"Binary Transition: {State}({Input})->{LeftState}|{RightState}" ;
    }
}