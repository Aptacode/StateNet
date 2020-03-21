using System;
using System.Collections.Generic;
using System.Linq;
using Aptacode.StateNet.Connections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aptacode.StateNet.Persistence.JSon
{
    public static class NetworkToJSon
    {
        public static readonly string StartStatePropertyName = "StartState";
        public static readonly string StatesPropertyName = "States";
        public static readonly string InputsPropertyName = "Inputs";
        public static readonly string ConnectionsPropertyName = "Connections";

        public static Network FromJSon(string jsonInput)
        {
            var network = new Network();

            var jObject = JObject.Parse(jsonInput);

            var startStateJson = jObject.Property(StartStatePropertyName).Value.ToString();
            var statesJson = jObject.Property(StatesPropertyName).Value.ToString();
            var inputsJson = jObject.Property(InputsPropertyName).Value.ToString();
            var connectionsJson = jObject.Property(ConnectionsPropertyName).Value.ToString();


            var startState = JsonConvert.DeserializeObject<State>(startStateJson);
            var states = JsonConvert.DeserializeObject<List<State>>(statesJson).ToList();
            var inputs = JsonConvert.DeserializeObject<List<Input>>(inputsJson).ToList();
            var connections = JsonConvert
                .DeserializeObject<List<Connection>>(connectionsJson).ToList();

            network.SetStart(startState);
            states.ForEach(state => network.CreateState(state));
            inputs.ForEach(input => network.CreateInput(input));
            connections.ForEach(connection => network.Connect(connection));

            return network;
        }

        public static string ToJson(Network network)
        {
            var jObject = new JObject();

            jObject.Add(StartStatePropertyName, JToken.FromObject(network.StartState));
            jObject.Add(StatesPropertyName, JToken.FromObject(network.GetStates()));
            jObject.Add(InputsPropertyName, JToken.FromObject(network.GetInputs()));
            jObject.Add(ConnectionsPropertyName, JToken.FromObject(network.GetConnections()));

            return jObject.ToString(Formatting.Indented);
        }

        public class StateConverter : JsonConverter<State>
        {
            public override void WriteJson(JsonWriter writer, State value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }

            public override State ReadJson(JsonReader reader, Type objectType, State existingValue,
                bool hasExistingValue, JsonSerializer serializer)
            {
                var s = (string) reader.Value;

                return new State(s);
            }
        }
    }
}