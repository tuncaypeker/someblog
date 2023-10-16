using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Unsplash.Model
{
    public class SearchApiResult
    {
        [JsonProperty("total")]
        public int TotalCount { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string description { get; set; }
        public string alt_description { get; set; }
        public int likes { get; set; }

        [JsonProperty("urls")]
        public ImageUrl ImageUrl { get; set; }
    }

    public class ImageUrl
    {
        public string raw { get; set; }
        public string full { get; set; }
        public string regular { get; set; }
        public string small { get; set; }
        public string thumb { get; set; }
    }
}
