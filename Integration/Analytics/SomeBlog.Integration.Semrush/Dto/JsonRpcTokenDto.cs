using Newtonsoft.Json;

namespace SomeBlog.Integration.Semrush.Dto
{
    public class JsonRpcTokenResult
    {
        public int exportLimit { get; set; }
        public bool hasProduct { get; set; }
        public bool isFilteringAllowed { get; set; }
        public bool isHistoryAllowed { get; set; }
        public bool isPLAAllowed { get; set; }
        public bool isPaid { get; set; }
        public bool isRootDomain { get; set; }
        public bool isSortingAllowed { get; set; }
        public bool isTrialAllowed { get; set; }
        public string productGroup { get; set; }
        public string token { get; set; }
    }

    public class JsonRpcTokenDto
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }

        [JsonProperty("Result")]
        public JsonRpcTokenResult result { get; set; }
    }
}
