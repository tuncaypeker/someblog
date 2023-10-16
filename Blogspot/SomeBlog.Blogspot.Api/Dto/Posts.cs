using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Blogspot.Api.Dto
{
    public class Posts
    {
        public string etag { get; set; }
        public string kind { get; set; }

        [JsonProperty("items")]
        public List<PostItem> posts { get; set; }
    }
}
