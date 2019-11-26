using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.States
{
    public class StateCollection : HashSet<State>
    {
        public StateCollection(IEnumerable<State> states) : base(states) { }

        public State this[string key] { get => this.FirstOrDefault(state => state.Name.Equals(key)); }
    }

    public class EnumStateCollection<TStates> : StateCollection
        where TStates : Enum
    {
        public EnumStateCollection() : base(Enum.GetNames(typeof(TStates)).Select(name => new State(name)).ToList()) { }

        public State this[TStates key] { get => this.FirstOrDefault(state => state.Name.Equals(key.ToString())); }
    }
}