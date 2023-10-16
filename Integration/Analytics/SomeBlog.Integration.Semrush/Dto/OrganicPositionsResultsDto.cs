using System.Collections.Generic;

namespace SomeBlog.Integration.Semrush.Dto
{
    public class OrganicPositionsResult
    {
        public int changeOfTraffic { get; set; }
        public double comp { get; set; }
        public double cpc { get; set; }
        public int crawledTime { get; set; }
        public int keywordDifficulty { get; set; }
        public List<int> keywordSerpFeatures { get; set; }
        public string phrase { get; set; }
        public int position { get; set; }
        public int positionDifference { get; set; }
        public List<int> positionSerpFeatures { get; set; }
        public int previousPosition { get; set; }
        public int results { get; set; }
        public List<int> serpFeatures { get; set; }
        public int traffic { get; set; }
        public int trafficCost { get; set; }
        public double trafficCostPercent { get; set; }
        public double trafficPercent { get; set; }
        public List<int> trends { get; set; }
        public string url { get; set; }
        public string urlHash { get; set; }
        public int volume { get; set; }
    }

    public class OrganicPositionsResultsDto
    {
        public string jsonrpc { get; set; }
        public int id { get; set; }
        public List<OrganicPositionsResult> result { get; set; }
        public int Count { get; set; }
    }
}
