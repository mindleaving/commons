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
            if (jToken.Type == JTokenType.Null)
                return null;
            string valueString = null;
            if (jToken.Type == JTokenType.String)
                valueString = jToken.Value<string>();
            else if (jToken is JObject jObject)
            {
                if (jObject.ContainsKey("StringValue"))
                {
                    if (jObject["StringValue"].Type == JTokenType.Null)
                        return null;
                    valueString = jObject["StringValue"].Value<string>();
                }
                else if (jObject.ContainsKey("unit") && jObject.ContainsKey("value"))
                {
                    var value = jToken.Value<string>("value");
                    var unit = jToken.Value<string>("unit");
                    valueString = $"{value} {unit}";
                }
            }
            if(valueString == null)
                throw new JsonReaderException("Invalid format of UnitValue");
            return UnitValueParser.Parse(valueString);
        }
    }
}
