using System;
using Newtonsoft.Json;
using Ordigo.API.Extensions;

namespace Ordigo.API.Serialization
{
    public class JsonHexColorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(nameof(WriteJson));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var hex = reader.Value.ToString();
            var color = hex.ToColor();

            return color;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
