using System;
using Commons.Physics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commons.Serialization
{
    public class UnitValueJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(UnitValue);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var unitValue = value as UnitValue;
            writer.WriteValue(unitValue?.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);
            string valueString;
            if (jToken.Type == JTokenType.String)
                valueString = jToken.Value<string>();
            else if (jToken is JObject jObject && jObject.ContainsKey("StringValue"))
                valueString = jObject["StringValue"].Value<string>();
            else
                throw new JsonReaderException("Invalid format of UnitValue");
            return UnitValueParser.Parse(valueString);
        }
    }
}
