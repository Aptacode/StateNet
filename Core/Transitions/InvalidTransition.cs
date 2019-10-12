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
    public class InvalidTransition<States, Actions> : Transition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public InvalidTransition(States state, Actions action, string message) : base(state, action, message)
        {

        }

        public override States Apply()
        {
            throw new InvalidTransitionException<States, Actions>(State, Action);
        }

        public override string ToString()
        {
            return string.Format("Invalid Transition: {0}({1})", Enum.GetName(typeof(States), State), Enum.GetName(typeof(Actions), Action));
        }
    }
}
