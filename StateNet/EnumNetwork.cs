using System;
using System.Collections.Generic;
using Aptacode.StateNet.Connections;

namespace Aptacode.StateNet
{
    public class EnumNetwork<TStates, TActions> : Network
        where TStates : Enum
        where TActions : Enum
    {
        public State this[TStates state] => this[state.ToString()];

        public IEnumerable<Connection> this[TStates state, TActions action] =>
            this[state.ToString(), action.ToString()];

        public State GetState(TStates state, bool createIfMissing = true)
        {
            return GetState(state.ToString(), createIfMissing);
        }

        public void Always(TStates fromState, TActions action, TStates toState)
        {
            Always(fromState.ToString(), action.ToString(), toState.ToString());
        }

        public void Clear(TStates fromState, TActions action, TStates toState)
        {
            Clear(fromState.ToString(), action.ToString(), toState.ToString());
        }

        public void Clear(TStates fromState, TActions action)
        {
            Clear(fromState.ToString(), action.ToString());
        }

        public void Clear(TStates fromState)
        {
            Clear(fromState.ToString());
        }
    }
}