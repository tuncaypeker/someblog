using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Pexels.Models
{
    public class CollectionMediaPage : Page
    {
        [JsonProperty("media")]
        public List<CollectionMedia> media { get; set; }
    }
}
