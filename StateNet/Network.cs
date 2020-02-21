using Aptacode.StateNet.Extensions;
using Aptacode.StateNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.ConnectionWeight;

namespace Aptacode.StateNet
{
    public class Network : INetwork
    {
        private readonly Dictionary<State, ConnectionGroup> _connections = new Dictionary<State, ConnectionGroup>();
        protected readonly Dictionary<string, State> _states = new Dictionary<string, State>();

        public Network()
        {
            ActOnFieldAndPropertyAttributes(typeof(StateNameAttribute), (memberInfo, attribute) =>
            {
                memberInfo.TrySetValue(this, GetState(((StateNameAttribute)attribute).Name));
            });

            ActOnFieldAndPropertyAttributes(typeof(StartStateAttribute), (memberInfo, attribute) =>
            {
                var state = GetState(((StartStateAttribute)attribute).Name);
                memberInfo.TrySetValue(this, state);
                StartState = state;
            });
            
            ActOnFieldAndPropertyAttributes(typeof(ConnectionAttribute), (field, attribute) =>
            {
                var connectionInfo = (ConnectionAttribute)attribute;
                field.TryGetValue(this, out State state);

                AddNewConnection(state.Name, connectionInfo.ActionName, connectionInfo.TargetName,
                    connectionInfo.ConnectionDescription);
            });
        }

        public ConnectionGroup this[State node] => GetConnection(node);

        public IEnumerable<State> GetAll()
        {
            return _states.Select(keyValue => keyValue.Value);
        }

        public IEnumerable<State> GetEndStates()
        {
            return _states.Select(keyValue => keyValue.Value).Where(IsEndNode);
        }

        public bool IsValid()
        {
            return GetEndStates().Any();
        }

        public State StartState { get; set; }

        public State this[string state] => GetState(state);

        public StateDistribution this[string state, string action] => GetConnection(GetState(state))[action];

        public StateDistribution this[State state, string action] => GetConnection(state)[action];

        public State GetState(string name, bool createIfMissing = true)
        {
            if (_states.TryGetValue(name, out var node))
            {
                return node;
            }

            if (!createIfMissing)
            {
                return null;
            }

            node = new State(name);
            _states.Add(name, node);

            return node;
        }

        public ConnectionGroup GetConnection(State node, bool createIfMissing = true)
        {
            if (_connections.TryGetValue(node, out var connectionGroup))
            {
                return connectionGroup;
            }

            if (!createIfMissing)
            {
                return null;
            }

            connectionGroup = new ConnectionGroup();
            _connections.Add(node, connectionGroup);

            return connectionGroup;
        }

        public bool IsEndNode(State state)
        {
            return GetConnection(state).GetAllDistributions().All(chooser => chooser.IsInvalid);
        }

        private void AddNewConnection(string startStateName, string actionName, string targetStateName,
            string connectionDescription = "")
        {
            this[startStateName, actionName].UpdateWeight(GetState(targetStateName),
                ConnectionWeightFactory.FromString(connectionDescription));
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

            foreach (var state in _states.Values.ToList())
            {
                stringBuilder.AppendLine(state.Name);

                var connectionGroups = GetConnection(state).GetAll();
                if (connectionGroups.Count == 0)
                {
                    continue;
                }

                stringBuilder.Append('(').Append(connectionGroups[0].Key).Append("->").Append(connectionGroups[0].Value)
                    .AppendLine(")");
                for (var i = 1; i < connectionGroups.Count; i++)
                {
                    stringBuilder.Append(",(").Append(connectionGroups[i].Key).Append("->")
                        .Append(connectionGroups[i].Value).AppendLine(")");
                }
            }

            return stringBuilder.ToString();
        }
    }
}