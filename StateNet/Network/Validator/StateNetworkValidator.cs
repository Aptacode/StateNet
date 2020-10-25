using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Engine.Interpreter.Expressions.Boolean;
using Aptacode.StateNet.Engine.Interpreter.Expressions.Integer;

namespace Aptacode.StateNet.Network.Validator
{
    public static class StateNetworkExtensions
    {
        public static StateNetworkValidationResult IsValid(this StateNetwork network)
        {
            if (string.IsNullOrEmpty(network.StartState))
            {
                return StateNetworkValidationResult.Fail("Start state was not set");
            }

            var connections = network.GetAllConnections();
            var states = network.GetAllStates();
            var inputs = network.GetAllInputs();
            var allStatesAndInputs = states.Concat(inputs);
            foreach (var connection in connections)
            {
                if (!states.Contains(connection.Target))
                {
                    return StateNetworkValidationResult.Fail("Connection target is not a valid state.");
                }

                var dependencies = new HashSet<string>();
                GetTransitionDependencies(connection.Expression, dependencies);

                var allDependenciesAreValid = dependencies.All(state => allStatesAndInputs.Contains(state));
                if (!allDependenciesAreValid)
                {
                    return StateNetworkValidationResult.Fail("Connection had an invalid state or input dependency.");
                }
            }

            var visitedStates = new HashSet<string>();
            var usableInputs = new HashSet<string>();
            GetVisitedStates(network, network.StartState, visitedStates, usableInputs);
            var unvisitedStates = states.Where(s => !visitedStates.Contains(s));

            if (unvisitedStates.Any())
            {
                return StateNetworkValidationResult.Fail("Unreachable states exist in the network.");
            }

            var unusedInputs = inputs.Where(s => !usableInputs.Contains(s));
            if (unusedInputs.Any())
            {
                return StateNetworkValidationResult.Fail("Unusable inputs exist in the network.");
            }

            return StateNetworkValidationResult.Ok("Success");
        }

        public static void GetVisitedStates(StateNetwork network, string state, HashSet<string> visitedStates,
            HashSet<string> usableInputs)
        {
            visitedStates.Add(state);
            var validInputs = network.GetInputs(state);
            foreach (var input in validInputs)
            {
                usableInputs.Add(input);
            }

            var connectedStates = validInputs
                .SelectMany(i => network.GetConnections(state, i))
                .Select(c => c.Target);

            var unvisitedConnectedStates =
                connectedStates.Distinct()
                    .Where(s => !visitedStates.Contains(s));

            foreach (var unvisitedState in unvisitedConnectedStates)
            {
                GetVisitedStates(network, unvisitedState, visitedStates, usableInputs);
            }
        }

        public static void GetTransitionDependencies(IIntegerExpression expression, HashSet<string> dependencies)
        {
            switch (expression)
            {
                case BinaryIntegerExpression binaryIntegerExpression:
                    GetTransitionDependencies(binaryIntegerExpression.LHS, dependencies);
                    GetTransitionDependencies(binaryIntegerExpression.RHS, dependencies);
                    break;
                case TernaryIntegerExpression ternaryIntegerExpression:
                    GetTransitionDependencies(ternaryIntegerExpression.Condition, dependencies);
                    GetTransitionDependencies(ternaryIntegerExpression.PassExpression, dependencies);
                    GetTransitionDependencies(ternaryIntegerExpression.FailExpression, dependencies);
                    break;
                case BaseTransitionHistoryMatchCount transitionHistoryMatchCount:
                    foreach (var dependency in transitionHistoryMatchCount.TransitionStringPattern)
                    {
                        if (string.IsNullOrEmpty(dependency))
                        {
                            continue;
                        }

                        dependencies.Add(dependency);
                    }

                    break;
            }
        }

        public static void GetTransitionDependencies(IBooleanExpression expression, HashSet<string> dependencies)
        {
            switch (expression)
            {
                case BinaryBooleanExpression binaryIntegerExpression:
                    GetTransitionDependencies(binaryIntegerExpression.LHS, dependencies);
                    GetTransitionDependencies(binaryIntegerExpression.RHS, dependencies);
                    break;
            }
        }
    }
}