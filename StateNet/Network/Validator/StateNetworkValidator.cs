using System.Collections.Generic;
using System.Linq;
using Aptacode.Expressions.Bool;
using Aptacode.Expressions.Integer;
using Aptacode.Expressions.List;
using Aptacode.StateNet.Engine.Expressions;
using Aptacode.StateNet.Engine.Transitions;

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

                var dependencies = new HashSet<string>();
                GetTransitionDependencies(connection.Expression, dependencies);

                var allDependenciesAreValid = dependencies.All(state => allStatesAndInputs.Contains(state));
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

        public static void GetTransitionDependencies(this IIntegerExpression<TransitionHistory> expression,
            HashSet<string> dependencies)
        {
            switch (expression)
            {
                case BinaryIntegerExpression<TransitionHistory> binaryIntegerExpression:
                    binaryIntegerExpression.Lhs.GetTransitionDependencies(dependencies);
                    binaryIntegerExpression.Rhs.GetTransitionDependencies(dependencies);
                    break;
                case TernaryIntegerExpression<TransitionHistory> ternaryIntegerExpression:
                    ternaryIntegerExpression.Condition.GetTransitionDependencies(dependencies);
                    ternaryIntegerExpression.PassExpression.GetTransitionDependencies(dependencies);
                    ternaryIntegerExpression.FailExpression.GetTransitionDependencies(dependencies);
                    break;
                case Count<TransitionHistory> count:
                    count.ListExpression.GetTransitionDependencies(dependencies);
                    break;
                case First<TransitionHistory> first:
                    first.Expression.GetTransitionDependencies(dependencies);
                    break;
                case Last<TransitionHistory> last:
                    last.Expression.GetTransitionDependencies(dependencies);
                    break;

                case Matches matches:

                    break;
            }
        }

        public static void GetTransitionDependencies(this IBooleanExpression<TransitionHistory> expression,
            HashSet<string> dependencies)
        {
            switch (expression)
            {
                case BinaryBoolExpression<TransitionHistory> binaryIntegerExpression:
                    binaryIntegerExpression.Lhs.GetTransitionDependencies(dependencies);
                    binaryIntegerExpression.Rhs.GetTransitionDependencies(dependencies);
                    break;
                case BinaryBoolComparison<TransitionHistory> binaryBoolComparison:
                    binaryBoolComparison.Lhs.GetTransitionDependencies(dependencies);
                    binaryBoolComparison.Rhs.GetTransitionDependencies(dependencies);
                    break;
            }
        }

        public static void GetTransitionDependencies(this IListExpression<TransitionHistory> expression,
            HashSet<string> dependencies)
        {
            switch (expression)
            {
                case Matches matches:
                    foreach (var dependency in matches.TransitionStringPattern)
                    {
                        if (string.IsNullOrEmpty(dependency))
                        {
                            continue;
                        }

                        dependencies.Add(dependency);
                    }

                    break;
                case TakeFirst<TransitionHistory> takeFirst:
                    takeFirst.Expression.GetTransitionDependencies(dependencies);
                    takeFirst.CountExpression.GetTransitionDependencies(dependencies);
                    break;
                case TakeLast<TransitionHistory> takeLast:
                    takeLast.Expression.GetTransitionDependencies(dependencies);
                    takeLast.CountExpression.GetTransitionDependencies(dependencies);
                    break;
                case UnaryListExpression<TransitionHistory> unaryExpression:
                    unaryExpression.Expression.GetTransitionDependencies(dependencies);
                    break;
                case BinaryListExpression<TransitionHistory> binaryExpression:
                    binaryExpression.Lhs.GetTransitionDependencies(dependencies);
                    binaryExpression.Rhs.GetTransitionDependencies(dependencies);
                    break;
                case TernaryIntegerExpression<TransitionHistory> ternaryIntegerExpression:
                    ternaryIntegerExpression.Condition.GetTransitionDependencies(dependencies);
                    ternaryIntegerExpression.PassExpression.GetTransitionDependencies(dependencies);
                    ternaryIntegerExpression.FailExpression.GetTransitionDependencies(dependencies);
                    break;
            }
        }

        public static void GetPatterns(this IIntegerExpression<TransitionHistory> expression,
            HashSet<int?[]> dependencies)
        {
            switch (expression)
            {
                case BinaryIntegerExpression<TransitionHistory> binaryIntegerExpression:
                    binaryIntegerExpression.Lhs.GetPatterns(dependencies);
                    binaryIntegerExpression.Rhs.GetPatterns(dependencies);
                    break;
                case TernaryIntegerExpression<TransitionHistory> ternaryIntegerExpression:
                    ternaryIntegerExpression.Condition.GetPatterns(dependencies);
                    ternaryIntegerExpression.PassExpression.GetPatterns(dependencies);
                    ternaryIntegerExpression.FailExpression.GetPatterns(dependencies);
                    break;
                case Count<TransitionHistory> count:
                    count.ListExpression.GetPatterns(dependencies);
                    break;
                case First<TransitionHistory> first:
                    first.Expression.GetPatterns(dependencies);
                    break;
                case Last<TransitionHistory> last:
                    last.Expression.GetPatterns(dependencies);
                    break;
            }
        }

        public static void GetPatterns(this IListExpression<TransitionHistory> expression,
            HashSet<int?[]> dependencies)
        {
            switch (expression)
            {
                case Matches matches:
                    dependencies.Add(matches.TransitionPattern);
                    break;
                case TakeFirst<TransitionHistory> takeFirst:
                    takeFirst.Expression.GetPatterns(dependencies);
                    takeFirst.CountExpression.GetPatterns(dependencies);
                    break;
                case TakeLast<TransitionHistory> takeLast:
                    takeLast.Expression.GetPatterns(dependencies);
                    takeLast.CountExpression.GetPatterns(dependencies);
                    break;
                case UnaryListExpression<TransitionHistory> unaryExpression:
                    unaryExpression.Expression.GetPatterns(dependencies);
                    break;
                case BinaryListExpression<TransitionHistory> binaryExpression:
                    binaryExpression.Lhs.GetPatterns(dependencies);
                    binaryExpression.Rhs.GetPatterns(dependencies);
                    break;
                case TernaryIntegerExpression<TransitionHistory> ternaryIntegerExpression:
                    ternaryIntegerExpression.Condition.GetPatterns(dependencies);
                    ternaryIntegerExpression.PassExpression.GetPatterns(dependencies);
                    ternaryIntegerExpression.FailExpression.GetPatterns(dependencies);
                    break;
            }
        }

        public static void GetPatterns(this IBooleanExpression<TransitionHistory> expression,
            HashSet<int?[]> dependencies)
        {
            switch (expression)
            {
                case BinaryBoolExpression<TransitionHistory> binaryIntegerExpression:
                    binaryIntegerExpression.Lhs.GetPatterns(dependencies);
                    binaryIntegerExpression.Rhs.GetPatterns(dependencies);
                    break;
                case BinaryBoolComparison<TransitionHistory> binaryBoolComparison:
                    binaryBoolComparison.Lhs.GetPatterns(dependencies);
                    binaryBoolComparison.Rhs.GetPatterns(dependencies);
                    break;
            }
        }
    }
}