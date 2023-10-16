using Newtonsoft.Json;
using SomeBlog.Integration.MozPro.Dto;
using System;

namespace SomeBlog.Integration.MozPro.Converters
{
    class SerpResultConverter : JsonConverter<Dto.SerpResultDto>
    {
        public override void WriteJson(JsonWriter writer, SerpResultDto value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override SerpResultDto ReadJson(JsonReader reader, Type objectType, SerpResultDto existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;

            return new SerpResultDto();
        }
    }
}
