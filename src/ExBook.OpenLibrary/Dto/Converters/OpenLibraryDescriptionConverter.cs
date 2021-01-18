using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

namespace ExBook.OpenLibrary.Dto
{
    internal class OpenLibraryDescriptionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object? ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return reader.Value as string;
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var json = JObject.Load(reader);
                JToken? value = json["value"];

                if (value?.Type == JTokenType.String)
                {
                    return value.ToString();
                }
            }

            return existingValue;
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}