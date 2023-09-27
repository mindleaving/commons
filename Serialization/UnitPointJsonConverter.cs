using System;
using Commons.Physics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Commons.Serialization
{
    public class UnitPointJsonConverter : JsonConverter<UnitPoint2D>
    {
        public override bool CanWrite => false;

        public override void WriteJson(
            JsonWriter writer,
            UnitPoint2D value,
            JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override UnitPoint2D ReadJson(
            JsonReader reader,
            Type objectType,
            UnitPoint2D existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            string unitString;
            if (!jObject.TryGetValue(nameof(UnitPoint2D.Unit), StringComparison.InvariantCultureIgnoreCase, out var unitToken))
                unitString = string.Empty;
            else if (unitToken.Type == JTokenType.Object)
            {
                var unitObject = (JObject)unitToken;
                if (unitObject.TryGetValue("UnitString", StringComparison.InvariantCultureIgnoreCase, out var unitStringToken))
                    unitString = unitStringToken.Value<string>();
                else
                    unitString = string.Empty;
            }
            else if (unitToken.Type == JTokenType.String)
                unitString = unitToken.Value<string>();
            else
                throw new JsonSerializationException("Unknown unit format");
            if (!jObject.TryGetValue(nameof(UnitPoint2D.X), StringComparison.InvariantCultureIgnoreCase, out var x))
                throw new JsonSerializationException("X is not specified");
            if(!jObject.TryGetValue(nameof(UnitPoint2D.Y), StringComparison.InvariantCultureIgnoreCase, out var y))
                throw new JsonSerializationException("Y is not specified");

            var parsedX = UnitValue.Parse(x.Value<double>() + unitString);
            var parsedY = UnitValue.Parse(y.Value<double>() + unitString);

            return new UnitPoint2D(parsedX, parsedY);
        }
    }
}
