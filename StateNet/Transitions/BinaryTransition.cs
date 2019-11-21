using Aptacode.StateNet.TransitionResults;
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
        /// <param name="message"></param>
        public BinaryTransition(string state,
                                string input,
                                string leftState,
                                string rightState,
                                Func<BinaryChoice> choiceCallback,
                                string message) : base(state, input, message)
        {
            LeftState = leftState;
            RightState = rightState;
            ChoiceCallback = choiceCallback;
        }

        protected Func<BinaryChoice> ChoiceCallback { get; set; }

        public override string Apply()
        {
            if((ChoiceCallback?.Invoke()) == BinaryChoice.Left)
            {
                return LeftState;
            }

            return RightState;
        }

        public override string ToString() => $"Binary Transition: {State}({Input})->{LeftState}|{RightState}" ;

        public string LeftState { get; }

        public string RightState { get; }
    }
}