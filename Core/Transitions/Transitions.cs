using System;

namespace Aptacode_StateMachine.StateNet.Core.Transitions
{
    /// <summary>
    /// An abstract generic class which represents a transition from 'State' With 'Action'
    /// </summary>
    /// <typeparam name="States">an Enum containing the available States</typeparam>
    /// <typeparam name="Actions">an Enum containing the available actions</typeparam>
    public abstract class Transition<States, Actions> where States : struct, Enum where Actions : struct, Enum
    {
        public States State { get; private set; }
        public Actions Action { get; private set; }
        public string Message { get; set; }

        protected Transition(States state, Actions action, string message)
        {
            State = state;
            Action = action;
            Message = message;
        }

        public abstract override string ToString();

        public abstract States Apply();
    }
}
