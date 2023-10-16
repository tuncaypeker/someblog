using Newtonsoft.Json;

namespace SomeBlog.Integration.Pexels.Models
{
    public class VideoPicture
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("picture")]
        public string picture { get; set; }

        [JsonProperty("nr")]
        public int nr { get; set; }
    }
}
