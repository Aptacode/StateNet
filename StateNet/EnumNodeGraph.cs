using Aptacode.StateNet.Connections;
using System;

namespace Aptacode.StateNet
{
    public class EnumNodeGraph<TStates, TActions> : NodeGraph
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

        public Node GetNode(TStates state)
        {
            if (!_nodes.TryGetValue(state.ToString(), out var node))
            {
                node = new Node(state.ToString());
                _nodes.Add(state.ToString(), node);
            }

            return node;
        }

        public Node this[TStates state] => GetNode(state);

        public NodeConnections this[TStates state, TActions action]
        {
            get => this[state.ToString(), action.ToString()];
        }
    }
}