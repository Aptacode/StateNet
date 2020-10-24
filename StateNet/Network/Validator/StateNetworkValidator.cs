using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

            foreach (var connection in connections)
            {
                if (!states.Contains(connection.Target))
                {
                    return StateNetworkValidationResult.Fail("Connection target is not a valid state.");
                }

                var dependencies = GetConnectionDependencies(connection);
                var validStates = dependencies.states.All(state => states.Contains(state));
                if (!validStates)
                {
                    return StateNetworkValidationResult.Fail("Connection had an invalid state dependency.");
                }

                var validInputs = dependencies.inputs.All(input => inputs.Contains(input));
                if (!validInputs)
                {
                    return StateNetworkValidationResult.Fail("Connection had an invalid input dependency.");
                }
            }

            return StateNetworkValidationResult.Ok("Success");
        }

        private static (IEnumerable<string> states, IEnumerable<string> inputs) GetConnectionDependencies(
            Connection connection)
        {
            var states = new List<string>();
            var inputs = new List<string>();

            var pattern = connection.Pattern;

            if (string.IsNullOrEmpty(pattern))
            {
                return (states, inputs);
            }

            var stateMatches = Regex.Matches(pattern, @"s<(.+?)>");
            foreach (Match stateMatch in stateMatches)
            {
                var captures = stateMatch.Groups;
                if (captures.Count < 1)
                {
                    continue;
                }

                var state = captures[1]?.Value;

                if (!string.IsNullOrEmpty(state))
                {
                    states.Add(state);
                }
            }

            var inputMatches = Regex.Matches(pattern, @"i<(.+?)>");
            foreach (Match inputMatch in inputMatches)
            {
                var captures = inputMatch.Groups;
                if (captures.Count < 1)
                {
                    continue;
                }

                var input = captures[1]?.Value;
                if (!string.IsNullOrEmpty(input))
                {
                    inputs.Add(input);
                }
            }

            return (states, inputs);
        }
    }
}