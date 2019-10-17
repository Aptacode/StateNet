namespace Aptacode.StateNet.Transitions
{
    public abstract class ValidTransition : Transition
    {
        protected ValidTransition(string state, string input, string message) : base(state, input, message)
        {
        }
    }
}