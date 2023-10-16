using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SomeBlog.Importer.Blogspot.Models
{
    public class PostItem
    {
        public string kind { get; set; }
        public string id { get; set; }
        public DateTime published { get; set; }
        public DateTime updated { get; set; }
        public string url { get; set; }
        public string selfLink { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public List<string> labels { get; set; }
        public string etag { get; set; }
    }

    public class Post
    {
        public string kind { get; set; }

        [JsonProperty("items")]
        public List<PostItem> posts { get; set; }
        public string etag { get; set; }
    }
}
