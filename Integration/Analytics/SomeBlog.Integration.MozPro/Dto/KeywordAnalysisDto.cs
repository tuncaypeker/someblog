using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.MozPro.Dto
{
    public class KeywordAnalysisDto
    {
        [JsonProperty("serp_id")]
        public int SerpId { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("opportunity")]
        public double Opportunity { get; set; }

        [JsonProperty("potential")]
        public double Potential { get; set; }

        [JsonProperty("exact_volume")]
        public double ExactVolume { get; set; }

        [JsonProperty("updated_at")]
        public System.DateTime UpdatedAt { get; set; }

        [JsonProperty("serp")]
        public KeywordAnalysisSerpResultDto Serp { get; set; }
    }

    public class KeywordAnalysisSerpResultDto
    {
        [JsonProperty("results")]
        //[JsonConverter(typeof(Converters.SerpResultConverter))]
        public SerpResultDto[] Results { get; set; }
    }
}
