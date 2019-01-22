using System;
using Commons.Physics;
using Newtonsoft.Json;

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
            return UnitValueParser.Parse(reader.Value as string);
        }
    }
}
