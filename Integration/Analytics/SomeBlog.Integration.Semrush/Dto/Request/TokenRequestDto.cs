using Newtonsoft.Json;

namespace SomeBlog.Integration.Semrush.Dto.Request
{
    public class TokenRequestParamsDto
    {
        [JsonProperty("reportType")]
        public string ReportType { get; set; }

        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("dateType")]
        public string DateType { get; set; }

        [JsonProperty("searchItem")]
        public string SearchItem { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }

    public class TokenRequestDto
    {
        public TokenRequestDto()
        {
            Id = 13;
            Jsonrpc = "2.0";
            Params = new TokenRequestParamsDto()
            {
                Database = "tr",
                DateType = "daily",
                PageSize = 100,
            };
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public TokenRequestParamsDto Params { get; set; }
    }
}
