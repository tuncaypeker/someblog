using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.Semrush.Dto
{
    public class Result
    {
        [JsonProperty("adwordsKeywords")]
        public int AdwordsKeywords { get; set; }

        [JsonProperty("commonKeywords")]
        public int CommonKeywords { get; set; }

        [JsonProperty("competitionLvl")]
        public double CompetitionLvl { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("organicKeywords")]
        public int OrganicKeywords { get; set; }

        [JsonProperty("traffic")]
        public int Traffic { get; set; }

        [JsonProperty("trafficCost")]
        public int TrafficCost { get; set; }
    }

    public class OrganicCompetitorsResultDto
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }


}
