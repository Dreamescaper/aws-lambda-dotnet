using Newtonsoft.Json;
using System;

namespace Amazon.Lambda.Serialization.Json
{
    internal class ObjectOrStringifiedEmptyObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && (string)reader.Value == "{}")
            {
                return Activator.CreateInstance(objectType);
            }

            return serializer.Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
