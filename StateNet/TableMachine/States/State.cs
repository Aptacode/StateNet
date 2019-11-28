using System;

namespace Aptacode.StateNet.TableMachine.States
{
    public struct State : IEquatable<State>
    {
        public State(string name) => Name = name;

        public override bool Equals(object obj) => (obj is State other) && this.Equals(other);

        public bool Equals(State other) => this.Name.Equals(other.Name);

        public override int GetHashCode() => Name.GetHashCode();

        public override string ToString() => Name;

        public string Name { get; }
    }
}
