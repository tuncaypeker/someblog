using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Pexels.Models
{
    public class PhotoPage : Page
    {
        [JsonProperty("photos")]
        public List<Photo> photos { get; set; }
    }
}
