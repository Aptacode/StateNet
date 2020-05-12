using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Connections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aptacode.StateNet.Json
{
    public class StateNetworkJsonConverter : JsonConverter
    {
        public static readonly string StartStatePropertyName = "StartState";
        public static readonly string StatesPropertyName = "States";
        public static readonly string InputsPropertyName = "Inputs";
        public static readonly string ConnectionsPropertyName = "Connections";
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(StateNetwork).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            return FromJSon(JObject.Load(reader));
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            ToJson(value as StateNetwork).WriteTo(writer);
        }

        public static IStateNetwork FromJSon(JObject jObject)
        {
            var network = new StateNetwork();
            var networkEditor = new StateNetworkEditor(network);

            var startStateJson = jObject.Property(StartStatePropertyName).Value.ToString();
            var statesJson = jObject.Property(StatesPropertyName).Value.ToString();
            var inputsJson = jObject.Property(InputsPropertyName).Value.ToString();
            var connectionsJson = jObject.Property(ConnectionsPropertyName).Value.ToString();


            var startState = JsonConvert.DeserializeObject<State>(startStateJson);
            var states = JsonConvert.DeserializeObject<List<State>>(statesJson).ToList();
            var inputs = JsonConvert.DeserializeObject<List<Input>>(inputsJson).ToList();
            var connections = JsonConvert
                .DeserializeObject<List<Connection>>(connectionsJson).ToList();

            networkEditor.SetStart(startState);
            states.ForEach(state => networkEditor.CreateState(state));
            inputs.ForEach(input => networkEditor.CreateInput(input));
            connections.ForEach(connection =>
                networkEditor.Connect(connection.Source, connection.Input, connection.Target,
                    connection.ConnectionWeight));

            return network;
        }

        public static JObject ToJson(IStateNetwork stateNetwork)
        {
            return new JObject
            {
                {StartStatePropertyName, JToken.FromObject(stateNetwork.StartState)},
                {StatesPropertyName, JToken.FromObject(stateNetwork.GetStates())},
                {InputsPropertyName, JToken.FromObject(stateNetwork.GetInputs())},
                {ConnectionsPropertyName, JToken.FromObject(stateNetwork.GetConnections())}
            };
        }
    }
}