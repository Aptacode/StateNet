using Aptacode.StateNet.Exceptions;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Transitions
{
    public class NaryTransition : ValidTransition
    {
        /// <summary>
        /// Defines a transition to the 'nextState' when the 'input' is applied to the current state
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="message"></param>
        public NaryTransition(string state,
                              string input,
                              List<string> nextStates,
                              Func<int> choiceCallback,
                              string message) : base(state, input, message)
        {
            NextStates = nextStates;
            ChoiceCallback = choiceCallback;
        }

        protected Func<int> ChoiceCallback { get; set; }

        /// <summary>
        /// Apply the transition
        /// </summary>
        /// <returns></returns>
        public override string Apply()
        {
            var choice = ChoiceCallback?.Invoke();

            if(choice.HasValue && (choice.Value < NextStates.Count))
            {
                return NextStates[choice.Value];
            }

            throw new AcceptanceCallbackFailedException(State, Input);
        }

        public override string ToString() => $"Nary Transition: {State}({Input})->{{ {string.Join("| ", NextStates)} }}" ;

        /// <summary>
        /// The output state of the unary transition
        /// </summary>
        public List<string> NextStates { get; }
    }
}