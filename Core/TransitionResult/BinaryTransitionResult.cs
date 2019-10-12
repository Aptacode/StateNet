namespace Aptacode.StateNet.Core.TransitionResult
{
    public enum BinaryChoice
    {
        Left, Right
    }

    public class BinaryTransitionResult : TransitionResult
    {
        public BinaryChoice Choice { get; set; }

        public BinaryTransitionResult(BinaryChoice choice, string message, bool success) : base(message, success)
        {
            Choice = choice;
        }

        public BinaryTransitionResult(BinaryChoice choice, string message) : this(choice, message, true)
        {

        }
    }
}
