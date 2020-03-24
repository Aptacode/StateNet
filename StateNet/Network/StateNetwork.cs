using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.Extensions;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet.Network
{
    public class StateNetwork : IStateNetwork
    {
        protected readonly Dictionary<string, Input> Inputs = new Dictionary<string, Input>();
        protected readonly Dictionary<string, State> States = new Dictionary<string, State>();

        public StateNetwork()
        {
            ActOnFieldAndPropertyAttributes(typeof(StateNameAttribute),
                (memberInfo, attribute) =>
                {
                    memberInfo.TrySetValue(this, GetState(((StateNameAttribute) attribute).Name));
                });

            ActOnFieldAndPropertyAttributes(typeof(StartStateAttribute), (memberInfo, attribute) =>
            {
                var state = GetState(((StartStateAttribute) attribute).Name);
                memberInfo.TrySetValue(this, state);
                StartState = state;
            });

            ActOnFieldAndPropertyAttributes(typeof(ConnectionAttribute), (field, attribute) =>
            {
                var connectionInfo = (ConnectionAttribute) attribute;
                field.TryGetValue(this, out State state);

                AddNewConnection(state.Name, connectionInfo.InputName, connectionInfo.TargetName,
                    connectionInfo.ConnectionDescription);
            });
        }

        public IEnumerable<Connection> GetConnections(string state)
        {
            return GetState(state).GetConnections();
        }

        public IEnumerable<Connection> this[string sourceState, string action] =>
            GetState(sourceState).GetConnections()
                .Where(connection => connection.Input.Equals(GetInput(action)));

        public Connection this[string sourceState, string action, string destinationState]
        {
            get
            {
                return GetConnections(sourceState)
                    .FirstOrDefault(connection =>
                        connection.Input.Equals(GetInput(action)) &&
                        connection.To.Equals(GetState(destinationState)));
            }
            set
            {
                var selectedState = GetState(sourceState);
                var oldConnection = this[sourceState, action, destinationState];
                if (oldConnection != null)
                {
                    selectedState.Remove(oldConnection);
                }

                if (value != null)
                {
                    selectedState.Add(value);
                }
            }
        }

        public IEnumerable<State> GetStates()
        {
            return States.Values.OrderBy(state => state.Name);
        }

        public IEnumerable<Input> GetInputs()
        {
            return Inputs.Values.OrderBy(input => input.Name);
        }

        public IEnumerable<Input> GetInputs(string state)
        {
            return GetConnections(state)
                .Select(connection => GetInput(connection.Input))
                .Distinct();
        }

        public IEnumerable<Connection> GetConnections()
        {
            return Connections.OrderBy(c => c.From.Name)
                .ThenBy(c => c.Input.Name)
                .ThenBy(c => c.To.Name);
        }

        public IEnumerable<State> GetEndStates()
        {
            return States.Values.Where(state => state.IsEnd());
        }

        public bool IsValid()
        {
            return GetEndStates().Any();
        }

        public IEnumerable<Connection> Connections =>
            States.Values.Select(state => state.GetConnections())
                .Aggregate((IEnumerable<Connection>) new List<Connection>(), (a, b) => a.Concat(b));

        public State StartState { get; private set; }

        public void SetStart(string state)
        {
            StartState = GetState(state);
        }

        public State this[string sourceState] => GetState(sourceState);

        public bool Equals(IStateNetwork other)
        {
            return other != null &&
                   GetInputs().SequenceEqual(other.GetInputs()) &&
                   GetStates().SequenceEqual(other.GetStates()) &&
                   Connections.SequenceEqual(other.Connections) &&
                   StartState.Equals(other.StartState);
        }

        public Input GetInput(string name, bool createIfMissing = true)
        {
            if (Inputs.TryGetValue(name, out var input))
            {
                return input;
            }

            if (!createIfMissing)
            {
                return null;
            }

            input = new Input(name);
            Inputs.Add(name, input);

            return input;
        }

        public State GetState(string name, bool createIfMissing = true)
        {
            if (States.TryGetValue(name, out var state))
            {
                return state;
            }

            if (!createIfMissing)
            {
                return null;
            }

            state = new State(name);
            States.Add(name, state);

            return state;
        }

        public State CreateState(string name)
        {
            return GetState(name);
        }

        public Input CreateInput(string name)
        {
            return GetInput(name);
        }

        public void Clear(string fromState)
        {
            GetState(fromState).GetConnections().ToList().ForEach(connection => connection?.From?.Remove(connection));
        }

        public void Clear(string fromState, string action)
        {
            GetState(fromState).GetConnections().Where(connection => connection.Input.Equals(GetInput(action))).ToList()
                .ForEach(connection => connection?.From?.Remove(connection));
        }

        public void Clear(string fromState, string action, string toState)
        {
            var connection = this[fromState, action, toState];
            connection?.From?.Remove(connection);
        }

        public void Always(string sourceState, string action, string toState)
        {
            Clear(sourceState, action);
            Connect(sourceState, action, toState, new ConnectionWeight(1.ToString()));
        }

        public void SetDistribution(string fromState, string input, params (string, int)[] choices)
        {
            Clear(fromState, input);
            UpdateDistribution(fromState, input, choices);
        }

        public void SetDistribution(string fromState, string input, params (string, ConnectionWeight)[] choices)
        {
            Clear(fromState, input);
            UpdateDistribution(fromState, input, choices);
        }

        public void UpdateDistribution(string fromState, string input, params (string, int)[] choices)
        {
            var connectionWeights = choices
                .Select(c => (c.Item1, new ConnectionWeight(c.Item2.ToString())))
                .ToArray();
            UpdateDistribution(fromState, input, connectionWeights);
        }

        public void UpdateDistribution(string fromState, string input,
            params (string, ConnectionWeight)[] choices)
        {
            foreach (var (toState, weight) in choices)
            {
                Clear(fromState, input, toState);
                Connect(fromState, input, toState, weight);
            }
        }

        public void RemoveState(string state)
        {
            States.Remove(state);

            var connections = Connections
                .Where(connection => connection.From.Name.Equals(state) || connection.To.Name.Equals(state))
                .ToList();

            connections.ForEach(connection => connection.From.Remove(connection));
        }

        public void RemoveInput(string input)
        {
            Inputs.Remove(input);

            var connections = Connections.Where(connection => connection.Input.Name.Equals(input)).ToList();
            connections.ForEach(connection => connection.From.Remove(connection));
        }

        public void Connect(string fromState, string input, string toState, ConnectionWeight connectionWeight = null)
        {
            connectionWeight = connectionWeight ?? new ConnectionWeight(1);
            this[fromState, input, toState] =
                new Connection(GetState(fromState), GetInput(input), GetState(toState), connectionWeight);
        }

        public IEnumerable<Connection> GetConnections(State state, string input)
        {
            return this[state, input];
        }

        public IEnumerable<State> GetOrderedStates()
        {
            return Traverse(StartState, state => GetConnections(state).Select(c => c.To));
        }

        public static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
        {
            var items = new List<T> {item};

            var stack = new Stack<T>();
            stack.Push(item);
            while (stack.Any())
            {
                var next = stack.Pop();

                foreach (var child in childSelector(next))
                {
                    if (!items.Contains(child))
                    {
                        items.Add(child);
                        stack.Push(child);
                    }
                }
            }

            return items;
        }

        private void AddNewConnection(string startStateName, string actionName, string targetStateName,
            string connectionDescription = "1")
        {
            Connect(startStateName, actionName, targetStateName, new ConnectionWeight(connectionDescription));
        }

        private void ActOnFieldAndPropertyAttributes(Type targetType, Action<MemberInfo, object> doWhenFound)
        {
            var typeInfo = GetType().GetTypeInfo();

            foreach (var fieldInfo in typeInfo.GetRuntimeFields())
            {
                foreach (var attr in fieldInfo.GetCustomAttributes(false))
                {
                    if (attr.GetType() == targetType)
                    {
                        doWhenFound(fieldInfo, attr);
                    }
                }
            }

            foreach (var propertyInfo in typeInfo.GetRuntimeProperties())
            {
                foreach (var attr in propertyInfo.GetCustomAttributes(false))
                {
                    if (attr.GetType() == targetType)
                    {
                        doWhenFound(propertyInfo, attr);
                    }
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("StartState");
            stringBuilder.Append("{");
            stringBuilder.Append(StartState);
            stringBuilder.Append("}");

            stringBuilder.AppendLine("States");
            stringBuilder.Append("{");
            stringBuilder.Append(string.Join(",", GetStates()));
            stringBuilder.Append("}");

            stringBuilder.AppendLine("Inputs");
            stringBuilder.Append("{");
            stringBuilder.Append(string.Join(",", GetInputs()));
            stringBuilder.Append("}");

            stringBuilder.AppendLine("Connections");
            stringBuilder.Append("{");
            stringBuilder.Append(string.Join(",", GetConnections()));
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }


        public override int GetHashCode()
        {
            return (Connections, States, Inputs, StartState).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is IStateNetwork other && Equals(other);
        }
    }
}