using Aptacode_StateMachine.StateNet.Core;
using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;

namespace Aptacode.StateNet.Core.Transitions
{
    /// <summary>
    /// A State Transition which is prohibited
    /// </summary>
    /// <typeparam name="States">an Enum containing the available States</typeparam>
    /// <typeparam name="Actions">an Enum containing the available actions</typeparam>
    public class InvalidTransition : Transition
    {
        public InvalidTransition(string state, string input, string message) : base(state, input, message)
        {

        }

        public override string Apply()
        {
            throw new InvalidTransitionException(State, Input);
        }

        public override string ToString()
        {
            return string.Format("Invalid Transition: {0}({1})", State, Input);
        }
    }
}
