using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aptacode.StateNet.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aptacode.StateNet.Persistence.Json
{
    public class StateNetworkJsonSerializer : INetworkSerializer
    {
        public static readonly string StartStatePropertyName = "StartState";
        public static readonly string StatesPropertyName = "States";
        public static readonly string InputsPropertyName = "Inputs";
        public static readonly string ConnectionsPropertyName = "Connections";

        public StateNetworkJsonSerializer(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; set; }

        public StateNetwork Read()
        {
            StateNetwork stateNetwork = null;

            using (var streamReader = new StreamReader(Filename))
            {
                stateNetwork = FromJSon(streamReader.ReadToEnd());
            }

            return stateNetwork;
        }

        public void Write(StateNetwork stateNetwork)
        {
            using (var streamWriter = new StreamWriter(Filename))
            {
                streamWriter.Write(ToJson(stateNetwork).ToString(Formatting.Indented));
            }
        }

        public static StateNetwork FromJSon(string jsonInput)
        {
            return FromJSon(JObject.Parse(jsonInput));
        }

        public static StateNetwork FromJSon(JObject jObject)
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
                networkEditor.Connect(connection.Source, connection.Input, connection.Target, connection.ConnectionWeight));

            return network;
        }

        public static JObject ToJson(StateNetwork stateNetwork)
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