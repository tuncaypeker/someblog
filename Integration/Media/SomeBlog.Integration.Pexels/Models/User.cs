using Newtonsoft.Json;

namespace SomeBlog.Integration.Pexels.Models
{
    public class User
    {

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("url")]
        public string url { get; set; }
    }
}
