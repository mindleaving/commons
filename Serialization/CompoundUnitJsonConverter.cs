using System;
using Commons.Physics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commons.Serialization
{
    public class CompoundUnitJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CompoundUnit);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var compoundUnit = value as CompoundUnit;
            writer.WriteValue(compoundUnit?.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);
            string valueString;
            if (jToken.Type == JTokenType.String)
                valueString = jToken.Value<string>();
            else if (jToken is JObject jObject && jObject.ContainsKey("UnitString"))
                valueString = jObject["UnitString"].Value<string>();
            else
                throw new JsonReaderException("Invalid format of CompoundUnit");
            return CompoundUnitParser.Parse(valueString);
        }
    }
}
