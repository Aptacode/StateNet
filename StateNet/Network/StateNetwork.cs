using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.Extensions;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Json;
using Aptacode.StateNet.Network.Connections;
using Newtonsoft.Json;

namespace Aptacode.StateNet.Network
{
    [JsonConverter(typeof(StateNetworkJsonConverter))]
    public class StateNetwork : IStateNetwork
    {
        public HashSet<Input> Inputs { get; } = new HashSet<Input>();
        public HashSet<State> States { get; } = new HashSet<State>();

        public StateNetwork()
        {
            ActOnFieldAndPropertyAttributes(typeof(StateNameAttribute),
                (memberInfo, attribute) =>
                {
                    memberInfo.TrySetValue(this, CreateState(((StateNameAttribute) attribute).Name));
                });

            ActOnFieldAndPropertyAttributes(typeof(StartStateAttribute), (memberInfo, attribute) =>
            {
                var state = CreateState(((StartStateAttribute) attribute).Name);
                memberInfo.TrySetValue(this, state);
                StartState = state;
            });

            ActOnFieldAndPropertyAttributes(typeof(ConnectionAttribute), (field, attribute) =>
            {
                var connectionInfo = (ConnectionAttribute) attribute;
                field.TryGetValue(this, out State state);

                Connect(
                    state.Name,
                    connectionInfo.Input,
                    connectionInfo.Target,
                    new ConnectionWeight(connectionInfo.Expression));
            });
        }

        /// <summary>
        ///     Returns true if the Network:
        ///     - Has a start state
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            var isStartStateValid = StartState != null;

            return isStartStateValid;
        }

        #region Attributes

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

        #endregion

        #region States

        public State StartState { get; set; }

        /// <summary>
        ///     Return the state with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public State GetState(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            States.TryGetValue(new State(name), out var state);
            return state;
        }

        /// <summary>
        ///     Return the state with the given name
        ///     Create new state if missing
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public State CreateState(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var newState = GetState(name);
            if (newState != null)
            {
                return newState;
            }

            newState = new State(name);
            States.Add(newState);
            return newState;
        }

        /// <summary>
        ///     Remove the state which matches the given name and all of its connections
        /// </summary>
        /// <param name="name"></param>
        public void RemoveState(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var state = new State(name);
            if (States.Contains(state))
            {
                States.Remove(state);
            }

            var connections = Connections
                .Where(connection =>
                    connection.Source.Name.Equals(name) ||
                    connection.Target.Name.Equals(name))
                .ToList();

            connections.ForEach(connection => connection.Source.Remove(connection));
        }

        public IEnumerable<State> GetOrderedStates()
        {
            if (StartState == null)
            {
                return GetStates();
            }

            var orderedStates = Traverse(StartState, state => GetConnections(state).Select(c => c.Target)).ToList();
            orderedStates.AddRange(GetStates().Where(s => !orderedStates.Contains(s)));

            return orderedStates;
        }

        private static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
        {
            var items = new List<T> {item};

            var stack = new Stack<T>();
            stack.Push(item);
            while (stack.Any())
            {
                var next = stack.Pop();

                foreach (var child in childSelector(next))
                {
                    if (items.Contains(child))
                    {
                        continue;
                    }

                    items.Add(child);
                    stack.Push(child);
                }
            }

            return items;
        }

        public IEnumerable<State> GetEndStates()
        {
            return States.Where(state => state.IsEnd());
        }

        public IEnumerable<State> GetStates()
        {
            return States.OrderBy(state => state.Name);
        }

        #endregion

        #region Inputs

        public IEnumerable<Input> GetInputs()
        {
            return Inputs.OrderBy(input => input.Name);
        }

        public IEnumerable<Input> GetInputs(string state)
        {
            return GetConnections(state)
                .Select(connection => GetInput(connection.Input))
                .Distinct();
        }

        public Input GetInput(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            Inputs.TryGetValue(new Input(name), out var input);
            return input;
        }

        public void RemoveInput(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var input = new Input(name);
            if (Inputs.Contains(input))
            {
                Inputs.Remove(input);
            }

            var connections = Connections.Where(connection => connection.Input.Name.Equals(input)).ToList();
            connections.ForEach(connection => connection.Source.Remove(connection));
        }

        public Input CreateInput(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var newInput = GetInput(name);
            if (newInput != null)
            {
                return newInput;
            }

            newInput = new Input(name);
            Inputs.Add(newInput);
            return newInput;
        }

        #endregion

        #region Connections

        public IEnumerable<Connection> Connections =>
            States.Select(state => state.Connections).SelectMany(list => list);

        public IEnumerable<Connection> GetConnections()
        {
            return Connections;
        }

        public IEnumerable<Connection> this[string sourceState] =>
            GetConnections(sourceState);

        public IEnumerable<Connection> this[string sourceState, string input] =>
            GetConnections(sourceState, input);

        public Connection this[string source, string input, string target]
        {
            get => GetConnection(source, input, target);
            set => Connect(value.Source, value.Input, value.Target, value.ConnectionWeight);
        }

        public void Connect(string source, string input, string target)
        {
            Connect(source, input, target, new ConnectionWeight(1));
        }

        public void Connect(string source, string input, string target, ConnectionWeight connectionWeight)
        {
            var selectedSource = CreateState(source);
            var selectedInput = CreateInput(input);
            var selectedTarget = CreateState(target);

            if (selectedSource == null || selectedInput == null || selectedTarget == null)
            {
                return;
            }

            var oldConnection = GetConnection(source, input, target);

            if (oldConnection != null)
            {
                selectedSource.Remove(oldConnection);
            }

            connectionWeight = connectionWeight ?? new ConnectionWeight(1);
            selectedSource.Add(new Connection(selectedSource, selectedInput, selectedTarget, connectionWeight));
        }

        public void Disconnect(string source, string input, string target)
        {
            var connection = GetConnection(source, input, target);
            connection?.Source?.Remove(connection);
        }

        public IEnumerable<Connection> GetConnections(string source)
        {
            return GetConnections().Where(connection => connection.Source.Name.Equals(source));
        }

        public IEnumerable<Connection> GetConnections(string source, string input)
        {
            return GetConnections(source).Where(connection => connection.Input.Name.Equals(input));
        }

        public Connection GetConnection(string source, string input, string target)
        {
            return GetConnections(source, input)
                .FirstOrDefault(connection => connection.Target.Name.Equals(target));
        }

        public IEnumerable<Connection> GetOrderedConnections()
        {
            return GetOrderedStates().Select(state => state.Connections).SelectMany(list => list);
        }

        #endregion

        #region Override

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("StartState");
            stringBuilder.Append("{");
            stringBuilder.Append(StartState);
            stringBuilder.Append("}");

            stringBuilder.AppendLine("States");
            stringBuilder.Append("{");
            stringBuilder.Append(string.Join(",", GetOrderedStates()));
            stringBuilder.Append("}");

            stringBuilder.AppendLine("Inputs");
            stringBuilder.Append("{");
            stringBuilder.Append(string.Join(",", GetInputs()));
            stringBuilder.Append("}");

            stringBuilder.AppendLine("Connections");
            stringBuilder.Append("{");
            stringBuilder.Append(string.Join(",", GetOrderedConnections()));
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }


        public override int GetHashCode()
        {
            return (Connections, States: States, Inputs: Inputs, StartState).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is IStateNetwork other && Equals(other);
        }

        public bool Equals(IStateNetwork other)
        {
            return other != null &&
                   GetInputs().SequenceEqual(other.GetInputs()) &&
                   GetStates().SequenceEqual(other.GetStates()) &&
                   Connections.SequenceEqual(other.Connections) &&
                   StartState.Equals(other.StartState);
        }

        #endregion
    }
}