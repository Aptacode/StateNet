using System;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class StateUpdatedEventArgs : EventArgs
    {
        public StateUpdatedEventArgs(State state)
        {
            State = state;
        }

        public State State { get; set; }

        public static StateUpdatedEventArgs None { get; } = new StateUpdatedEventArgs(null);

        #region Overrides

        public override int GetHashCode()
        {
            return State.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is StateUpdatedEventArgs other && Equals(other);
        }

        public bool Equals(StateUpdatedEventArgs other)
        {
            return other != null && State.Equals(other.State);
        }

        #endregion Overrides
    }
}