using System;
using Commons.Extensions;
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
            var unitConversionResult = CompoundUnitParser.Parse(1d, valueString);
            if((unitConversionResult.Value - 1).Abs() > 1e-12)
                throw new JsonReaderException("Invalid format of CompoundUnit. Must be SI units");
            return unitConversionResult.Unit;
        }
    }
}
