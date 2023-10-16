using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Semrush.Dto
{
    public class KeywordSerpResult
    {
        public string domain { get; set; }
        public string url { get; set; }
        public int position { get; set; }
    }

    public class KeywordSerpDto
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }

        [JsonProperty("Result")]
        public List<KeywordSerpResult> result { get; set; }
    }
}
