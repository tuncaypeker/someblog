using Newtonsoft.Json;

namespace SomeBlog.Integration.Pexels.Models
{
    public class Source
    {
        [JsonProperty("original")]
        public string original { get; set; }

        [JsonProperty("large")]
        public string large { get; set; }

        [JsonProperty("large2x")]
        public string large2x { get; set; }

        [JsonProperty("medium")]
        public string medium { get; set; }

        [JsonProperty("small")]
        public string small { get; set; }

        [JsonProperty("portrait")]
        public string portrait { get; set; }

        [JsonProperty("landscape")]
        public string landscape { get; set; }

        [JsonProperty("tiny")]
        public string tiny { get; set; }
    }
}
