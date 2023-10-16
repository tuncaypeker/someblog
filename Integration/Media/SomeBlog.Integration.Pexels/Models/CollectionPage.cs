using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Pexels.Models
{
    public class CollectionPage : Page
    {
        [JsonProperty("collections")]
        public List<Collection> collections { get; set; }
    }
}
