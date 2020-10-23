using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.Random;

namespace Aptacode.StateNet.Engine {
    public class StateNetEngine
    {
        private readonly Network.StateNetwork _network;
        private readonly TransitionHistory _transitionHistory = new TransitionHistory();
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public StateNetEngine(Network.StateNetwork network, IRandomNumberGenerator randomNumberGenerator)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
            CurrentState = _network.StartState;
        }

        public string CurrentState { get; private set; }

        public event EventHandler<Transition>? OnTransition;

        public TransitionResult Apply(string input)
        {
           var connections = _network.GetConnections(CurrentState, input);

            if (connections == null)
                return TransitionResult.Fail($"There are no available connections from {CurrentState} for {input}.");

           var weightedConnections = new List<string>();
           foreach (var connection in connections)
           {
               var connectionWeight = connection.GetWeight(_transitionHistory);
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
                return TransitionResult.Fail($"There are no connections with a positive weight {CurrentState} for {input}.");
           }

            var connectionIndex = _randomNumberGenerator.Generate(0, weightedConnections.Count);
           var nextState = weightedConnections.ElementAt(connectionIndex);

           var transition = new Transition(CurrentState, input, nextState);
           _transitionHistory.Add(transition);

            CurrentState = nextState;

           OnTransition?.Invoke(this, transition);

            return TransitionResult.Ok(transition, "Success.");
        }
    }
}