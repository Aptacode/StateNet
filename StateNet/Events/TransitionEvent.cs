namespace Aptacode.StateNet.Events
{
    public delegate void TransitionEvent(Engine sender, State fromState, Input input, State toState);
}