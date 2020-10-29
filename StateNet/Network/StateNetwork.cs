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
            if (string.IsNullOrEmpty(startState))
            {
                throw new ArgumentNullException(nameof(startState));
            }

            _stateDictionary = stateDictionary ?? throw new ArgumentNullException(nameof(stateDictionary));

            if (!_stateDictionary.Keys.Any())
            {
                throw new ArgumentException(nameof(stateDictionary));
            }

            ;

            StartState = startState;
        }

        public string StartState { get; set; }

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

        public IReadOnlyList<Connection> GetAllConnections()
        {
            return _stateDictionary.Values.SelectMany(c => c.Values.SelectMany(c => c)).ToList();
        }

        public IReadOnlyList<string> GetAllInputs()
        {
            return _stateDictionary.Values.SelectMany(c => c.Keys).ToList();
        }

        public IReadOnlyList<string> GetAllStates() => _stateDictionary.Keys.ToList();
    }
}