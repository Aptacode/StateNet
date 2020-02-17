using System;

namespace Aptacode.StateNet
{
    public class EnumNetwork<TStates, TActions> : Network
        where TStates : Enum
        where TActions : Enum
    {
        public void Set(
            TStates source,
            TActions action,
            TStates target)
        {
            var targetNode = GetNode(target);

            this[source.ToString(), action.ToString()].Always(targetNode);
        }

        public void Set(TStates source, TActions action, params (TStates, int)[] targetChoices)
        {
            foreach (var pair in targetChoices)
            {
                var targetNode = GetNode(pair.Item1);
                this[source.ToString(), action.ToString()].UpdateWeight(targetNode, pair.Item2);
            }
        }

        public void UpdateWeight(TStates source, TActions action, TStates targetState, int targetWeight) => this[source.ToString(), action.ToString()].UpdateWeight(GetNode(targetState), targetWeight);

        public State GetNode(TStates state)
        {
            if (!_states.TryGetValue(state.ToString(), out var node))
            {
                node = new State(state.ToString());
                _states.Add(state.ToString(), node);
            }

            return node;
        }

        public State this[TStates state] => GetNode(state);

        public StateDistribution this[TStates state, TActions action]
        {
            get => this[state.ToString(), action.ToString()];
        }
    }
}