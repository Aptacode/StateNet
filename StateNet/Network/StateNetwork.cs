using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.Network {
    public class StateNetwork
    {
        private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>> _stateDictionary;

        public StateNetwork(IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>> stateDictionary, string startState)
        {
            _stateDictionary = stateDictionary ?? throw new ArgumentNullException(nameof(stateDictionary));
            StartState = startState;
        }

        public string StartState { get; }

        public IReadOnlyList<Connection>? GetConnections(string state, string input)
        {
            if (!_stateDictionary.TryGetValue(state, out var inputs))
            {
                return null;
            }

            return inputs.TryGetValue(input, out var connections) ? connections : null;
        }
    }
}