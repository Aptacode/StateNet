﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Aptacode.StateNet.Attributes;
using Aptacode.StateNet.ConnectionWeight;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{
    public class Network : INetwork
    {
        private readonly Dictionary<State, ConnectionGroup> _connections = new Dictionary<State, ConnectionGroup>();
        protected readonly Dictionary<string, State> _states = new Dictionary<string, State>();

        public Network()
        {
            //TODO - figure out a clean way to use fields and properties in the same
            //assignment operation. They are both MemberInfos, but that's too restrictive with no SetValue()
            //
            //Can't just use: https://stackoverflow.com/questions/2004508/checking-type-parameter-of-a-generic-method-in-c-sharp
            //You still duplicate castings and logic for both fields and properties.
            //
            //Consider creating a MemberInfo extension method that checks for fields and properties, casts as needed,
            //and sets a value if appropriate. Eg memberInfo.TrySetValue("NewValue");
            ActOnFieldAttributes(typeof(StateNameAttribute),
                (field, attribute) => { field.SetValue(this, GetState(((StateNameAttribute) attribute).Name)); });

            ActOnPropertyAttributes(typeof(StateNameAttribute),
                (property, attribute) => { property.SetValue(this, GetState(((StateNameAttribute) attribute).Name)); });

            ActOnFieldAttributes(typeof(StartStateAttribute), (field, attribute) =>
            {
                var state = GetState(((StartStateAttribute) attribute).Name);
                field.SetValue(this, state);
                StartState = state;
            });

            ActOnPropertyAttributes(typeof(StartStateAttribute), (property, attribute) =>
            {
                var state = GetState(((StartStateAttribute) attribute).Name);
                property.SetValue(this, state);
                StartState = state;
            });

            ActOnFieldAttributes(typeof(ConnectionAttribute), (field, attribute) =>
            {
                var connectionInfo = (ConnectionAttribute) attribute;
                var state = (State) field.GetValue(this);

                AddNewConnection(state.Name, connectionInfo.ActionName, connectionInfo.TargetName,
                    connectionInfo.ConnectionDescription);
            });

            ActOnPropertyAttributes(typeof(ConnectionAttribute), (property, attribute) =>
            {
                var connectionInfo = (ConnectionAttribute) attribute;
                var state = (State) property.GetValue(this);

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

        private void ActOnFieldAttributes(Type targetType, Action<FieldInfo, object> doWhenFound)
        {
            var typeInfo = GetType().GetTypeInfo();

            foreach (var fieldInfo in typeInfo.GetRuntimeFields())
            {
                foreach (var attr in fieldInfo.GetCustomAttributes(true))
                {
                    if (attr.GetType() == targetType)
                    {
                        doWhenFound(fieldInfo, attr);
                    }
                }
            }
        }

        private void ActOnPropertyAttributes(Type targetType, Action<PropertyInfo, object> doWhenFound)
        {
            var typeInfo = GetType().GetTypeInfo();

            foreach (var propertyInfo in typeInfo.GetRuntimeProperties())
            {
                foreach (var property in propertyInfo.GetCustomAttributes(true))
                {
                    if (property.GetType() == targetType)
                    {
                        doWhenFound(propertyInfo, property);
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
                if (!connectionGroups.Any())
                {
                    continue;
                }

                stringBuilder.AppendLine($"({connectionGroups[0].Key}->{connectionGroups[0].Value})");
                for (var i = 1; i < connectionGroups.Count; i++)
                {
                    stringBuilder.AppendLine($",({connectionGroups[i].Key}->{connectionGroups[i].Value})");
                }
            }

            return stringBuilder.ToString();
        }
    }
}