using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Random;

namespace Aptacode.StateNet.Engine
{
    public class StateNetEngine
    {
        private readonly StateNetwork _network;
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public readonly TransitionHistory TransitionHistory;

        public StateNetEngine(StateNetwork network, IRandomNumberGenerator randomNumberGenerator)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _randomNumberGenerator =
                randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
            CurrentState = _network.StartState;
            TransitionHistory = new TransitionHistory(_network);
        }

        public string CurrentState { get; private set; }

        public event EventHandler<Transition>? OnTransition;

        public IReadOnlyList<string> GetAvailableInputs() => _network.GetInputs(CurrentState);

        public IReadOnlyList<Connection> GetAvailableConnections(string input) =>
            _network.GetConnections(CurrentState, input);

        public TransitionResult Apply(string input)
        {
            var connections = _network.GetConnections(CurrentState, input);

            if (connections.Count == 0)
            {
                return TransitionResult.Fail(Resources.NO_AVAILABLE_CONNECTION(CurrentState, input));
            }

            var weightedConnections = new List<string>();
            foreach (var connection in connections)
            {
                var connectionWeight = connection.Expression.Interpret(TransitionHistory);
                if (connectionWeight <= 0)
                {
                    continue;
                }

                for (var i = 0; i < connectionWeight; i++)
                {
                    weightedConnections.Add(connection.Target);
                }
            }

            if (weightedConnections.Count == 0)
            {
                return TransitionResult.Fail(Resources.NO_AVAILABLE_CONNECTION(CurrentState, input));
            }

            var connectionIndex = _randomNumberGenerator.Generate(0, weightedConnections.Count);
            var nextState = weightedConnections.ElementAt(connectionIndex);
            var transition = new Transition(CurrentState, input, nextState);
            TransitionHistory.Add(transition.Input, transition.Destination);

            CurrentState = nextState;

            OnTransition?.Invoke(this, transition);

            return TransitionResult.Ok(transition, Resources.SUCCESS);
        }
    }
}