using System;
using Aptacode.StateNet.Network;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class SelectedStateEventArgs : EventArgs, IEquatable<SelectedStateEventArgs>
    {
        public SelectedStateEventArgs(State state)
        {
            State = state;
        }

        public State State { get; set; }

        public static SelectedStateEventArgs None { get; } = new SelectedStateEventArgs(null);

        #region Overrides

        public override int GetHashCode()
        {
            return State.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is SelectedStateEventArgs other && Equals(other);
        }

        public bool Equals(SelectedStateEventArgs other)
        {
            return other != null && State.Equals(other.State);
        }

        #endregion Overrides
    }
}