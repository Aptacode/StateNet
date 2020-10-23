using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.Network
{
    public sealed class StateNetwork
    {
        private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>>
            _stateDictionary;

        public StateNetwork(
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Connection>>> stateDictionary,
            string startState)
        {
            _stateDictionary = stateDictionary ?? throw new ArgumentNullException(nameof(stateDictionary));
            if (string.IsNullOrEmpty(startState))
            {
                throw new ArgumentNullException(nameof(startState));
            }

            StartState = startState;
        }

        public string StartState { get; }

        public IReadOnlyList<Connection> GetConnections(string state, string input)
        {
            if (!_stateDictionary.TryGetValue(state, out var inputs))
            {
                return new Connection[0];
            }

            return inputs.TryGetValue(input, out var connections) ? connections : new Connection[0];
        }

        public IReadOnlyList<string> GetInputs(string state)
        {
            if (!_stateDictionary.TryGetValue(state, out var inputs))
            {
                return new string[0];
            }

            return inputs.Keys.ToList();
        }
    }
}