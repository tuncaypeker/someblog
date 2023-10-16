using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.KeywordsEveryWhere
{
    public class Cpc
    {
        public string currency { get; set; }
        public decimal value { get; set; }
    }

    public class Trend
    {
        public string month { get; set; }
        public int year { get; set; }
        public int value { get; set; }
    }

    public class Result
    {
        public int vol { get; set; }
        public Cpc cpc { get; set; }
        public string keyword { get; set; }
        public decimal competition { get; set; }
        public List<Trend> trend { get; set; }
    }

    public class KeywordsApiModel
    {
        [JsonProperty("data")]
        public List<Result> data { get; set; }
        public int credits { get; set; }
        public decimal time { get; set; }
    }
}
