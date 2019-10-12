﻿namespace Aptacode_StateMachine.StateNet.Core
{
    /// <summary>
    /// information given back to a transition relating to the execution of the action to determine which state to move to
    /// </summary>
    public abstract class TransitionAcceptanceResult
    {
        //The message returned from the transition
        public string Message { get; set; }
        //The State returned from the transition
        public bool Failed { get; set; }
        protected TransitionAcceptanceResult(string message, bool failed)
        {
            Message = message;
            Failed = failed;
        }
    }

    public class UnaryTransitionAcceptanceResult : TransitionAcceptanceResult
    {
        public UnaryTransitionAcceptanceResult(string message, bool failed = false) : base(message, failed)
        {

        }

        public UnaryTransitionAcceptanceResult(string message) : base(message, false)
        {

        }
    }

    public class BinaryTransitionAcceptanceResult : TransitionAcceptanceResult
    {
        public enum BinaryChoice
        {
            Left, Right
        }
        public BinaryChoice Choice { get; set; }

        public BinaryTransitionAcceptanceResult(BinaryChoice choice, string message, bool failed) : base(message, failed)
        {
            Choice = choice;
        }

        public BinaryTransitionAcceptanceResult(BinaryChoice choice, string message) : this(choice, message, false)
        {

        }
    }
}
