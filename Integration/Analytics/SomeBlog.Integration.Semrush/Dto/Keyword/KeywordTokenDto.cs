using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Semrush.Dto
{
    public class KeywordTokenResult
    {
        public string token { get; set; }
        public List<object> keywords { get; set; }
    }

    public class KeywordTokenDto
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }

        [JsonProperty("Result")]
        public KeywordTokenResult result { get; set; }
    }
}
