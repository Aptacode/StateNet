using System;
using Aptacode.StateNet.Connections.Weights;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aptacode.StateNet.Persistence.JSon
{
    public class ConnectionWeightConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return typeof(ConnectionWeight).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);


            var connectionType = jo[nameof(ConnectionWeight.TypeName)].ToString();

            ConnectionWeight item;
            if (connectionType == nameof(StaticWeight))
            {
                item = new StaticWeight();
            }
            else if (connectionType == nameof(VisitCountWeight))
            {
                item = new VisitCountWeight();
            }
            else
            {
                return null;
            }

            serializer.Populate(jo.CreateReader(), item);

            return item;
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}