using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Pexels.Models
{
    public class VideoPage : Page
    {
        [JsonProperty("videos")]
        public IEnumerable<Video> videos { get; set; }
    }
}
