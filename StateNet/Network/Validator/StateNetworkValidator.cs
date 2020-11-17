using System.Collections.Generic;
using System.Linq;
using Aptacode.Expressions.Bool;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Transitions;
using Aptacode.StateNet.PatternMatching;
using Aptacode.StateNet.PatternMatching.Expressions;

namespace Aptacode.StateNet.Network.Validator
{
    public static class StateNetworkExtensions
    {
        public static StateNetworkValidationResult IsValid(this StateNetwork network)
        {
            if (string.IsNullOrEmpty(network.StartState))
            {
                return StateNetworkValidationResult.Fail(Resources.UNSET_START_STATE);
            }

            var connections = network.GetAllConnections();
            var states = network.GetAllStates();

            if (!states.Contains(network.StartState))
            {
                return StateNetworkValidationResult.Fail(Resources.INVALID_START_STATE);
            }

            var inputs = network.GetAllInputs();
            var allStatesAndInputs = states.Concat(inputs);
            foreach (var connection in connections)
            {
                if (!states.Contains(connection.Target))
                {
                    return StateNetworkValidationResult.Fail(Resources.INVALID_CONNECTION_TARGET);
                }

                var matchesVisitor = new MatchesVisitor();
                matchesVisitor.Schedule(connection.Expression);

                var allDependenciesAreValid = matchesVisitor.Dependencies.All(state => allStatesAndInputs.Contains(state));
                if (!allDependenciesAreValid)
                {
                    return StateNetworkValidationResult.Fail(Resources.INVALID_DEPENDENCY);
                }
            }

            var visitedStates = new HashSet<string>();
            var usableInputs = new HashSet<string>();
            GetVisitedStatesAndUsableInputs(network, network.StartState, visitedStates, usableInputs);
            var unvisitedStates = states.Where(s => !visitedStates.Contains(s));

            if (unvisitedStates.Any())
            {
                return StateNetworkValidationResult.Fail(Resources.UNREACHABLE_STATES);
            }

            var unusedInputs = inputs.Where(s => !usableInputs.Contains(s));
            return unusedInputs.Any()
                ? StateNetworkValidationResult.Fail(Resources.UNUSABLE_INPUTS)
                : StateNetworkValidationResult.Ok(Resources.SUCCESS);
        }

        private static IEnumerable<string> GetUsableInputs(StateNetwork network, string state,
            IReadOnlyList<string> inputs)
        {
            return inputs.Where(input => network.GetConnections(state, input).Any());
        }

        public static void GetVisitedStatesAndUsableInputs(StateNetwork network, string state,
            HashSet<string> visitedStates,
            HashSet<string> usableInputs) //This is quite big and could maybe be separated out?
        {
            visitedStates.Add(state);
            var inputs = network.GetInputs(state);
            foreach (var input in GetUsableInputs(network, state, inputs)
            ) //This defines a valid input as one which has connections, not sure if this is a strong enough definition.
            {
                usableInputs.Add(input);
            }

            var connectedStates = inputs
                .SelectMany(i => network.GetConnections(state, i))
                .Select(c => c.Target);

            var unvisitedConnectedStates =
                connectedStates.Distinct()
                    .Where(s => !visitedStates.Contains(s));

            foreach (var unvisitedState in unvisitedConnectedStates)
            {
                GetVisitedStatesAndUsableInputs(network, unvisitedState, visitedStates, usableInputs);
            }
        }
    }
}