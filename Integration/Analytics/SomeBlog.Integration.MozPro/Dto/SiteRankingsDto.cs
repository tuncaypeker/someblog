using Newtonsoft.Json;
using System.Collections.Generic;

namespace SomeBlog.Integration.MozPro.Dto
{
    public class SiteRankingsDto
    {
        [JsonProperty("data")]
        public SiteRankingsData Data { get; set; }
    }

    public class SiteRankingsData
    {
        [JsonProperty("keyword_rankings")]
        public List<KeywordRanking> KeywordRankings { get; set; }

        public int count { get; set; }
    }

    public class KeywordRanking
    {
        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        [JsonProperty("exact_volume")]
        public double ExactVolume { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("subjects")]
        public List<KeywordRankingSubject> Subject { get; set; }
    }

    public class KeywordRankingSubject
    {
        [JsonProperty("rankings")]
        public List<KeywordSiteRanking> SiteRankings { get; set; }
    }

    public class KeywordSiteRanking
    {
        public string url { get; set; }
        public int rank { get; set; }
    }
}
