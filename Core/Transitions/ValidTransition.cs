using Aptacode_StateMachine.StateNet.Core.Transitions;
using System;

namespace Aptacode.StateNet.Core.Transitions
{
    /// <summary>
    /// Represents an abstract valid state transition, this class must be inherited to define the state to transition to.
    /// </summary>
    /// <typeparam name="State"></typeparam>
    /// <typeparam name="Action"></typeparam>
    public abstract class ValidTransition : Transition
    {
        protected ValidTransition(string state, string input, string message) : base(state, input, message)
        {

        }
    }
}
