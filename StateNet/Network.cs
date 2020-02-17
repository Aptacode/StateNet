using Aptacode.StateNet.Events.Attributes;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.NodeWeights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Aptacode.StateNet
{
    public class Network : INetwork
    {
        protected readonly Dictionary<string, State> _states = new Dictionary<string, State>();
        private readonly Dictionary<State, ConnectionGroup> _connections = new Dictionary<State, ConnectionGroup>();

        public IEnumerable<State> GetAll() => _states.Select(keyValue => keyValue.Value);

        public IEnumerable<State> GetEndStates() => _states.Select(keyValue => keyValue.Value).Where(IsEndNode);

        public bool IsValid() => GetEndStates().Any();

        public State StartState { get; set; }

        public State GetState(string name)
        {
            if (!_states.TryGetValue(name, out var node))
            {
                node = new State(name);
                _states.Add(name, node);
            }

            return node;
        }

        public ConnectionGroup GetConnection(State node)
        {
            if (!_connections.TryGetValue(node, out var connectionGroup))
            {
                connectionGroup = new ConnectionGroup();
                _connections.Add(node, connectionGroup);
            }

            return connectionGroup;
        }

        public ConnectionGroup this[State node] => GetConnection(node);

        public State this[string state] => GetState(state);

        public StateDistribution this[string state, string action] => GetConnection(GetState(state))[action];
        
        public StateDistribution this[State state, string action] => GetConnection(state)[action];
        
        public bool IsEndNode(State state) => GetConnection(state).GetAllDistributions().All((chooser) => chooser.IsInvalid);

        public Network()
        {
            ActOnFieldAttributes(typeof(StateNameAttribute), (field, attribute) =>
            {
                field.SetValue(this, GetState(((StateNameAttribute)attribute).Name));
            });

            ActOnFieldAttributes(typeof(StartStateAttribute), (field, attribute) =>
            {
                var state = GetState(((StartStateAttribute)attribute).Name);
                field.SetValue(this, state);
                StartState = state;
            });

            ActOnFieldAttributes(typeof(ConnectionAttribute), (field, attribute) =>
            {
                var connectionInfo = (ConnectionAttribute)attribute;
                var state = (State)field.GetValue(this);

                AddNewConnection(state.Name, connectionInfo.ActionName, connectionInfo.TargetName, connectionInfo.ConnectionDescription);
                
            });
        }

        private void AddNewConnection(string startStateName, string actionName, string targetStateName, string connectionDescription = "")
        {
            this[startStateName, actionName].UpdateWeight(GetState(targetStateName), ConnectionWeightFactory.FromString(connectionDescription));
        }

        private void ActOnFieldAttributes(Type targetType, Action<FieldInfo, object> doWhenFound)
        {
            var typeInfo = GetType().GetTypeInfo();

            foreach (var field in typeInfo.GetRuntimeFields())
            {
                foreach (var attr in field.GetCustomAttributes(true))
                {
                    if (attr.GetType() == targetType)
                    {
                        doWhenFound(field, attr);
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
                if (connectionGroups.Any())
                {
                    stringBuilder.AppendLine($"({connectionGroups[0].Key}->{connectionGroups[0].Value})");
                    for (var i = 1; i < connectionGroups.Count; i++)
                    {
                        stringBuilder.AppendLine($",({connectionGroups[i].Key}->{connectionGroups[i].Value})");
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}