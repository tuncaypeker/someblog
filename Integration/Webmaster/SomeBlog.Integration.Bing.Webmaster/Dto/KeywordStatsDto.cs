using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class KeywordStatsDto
    {
        public long BroadImpressions;
        public System.DateTime Date;
        public long Impressions;
        public string Query;

        public static KeywordStatsDto From(KeywordStats x)
        {
            return new KeywordStatsDto()
            {
              BroadImpressions = x.BroadImpressions,
              Date = x.Date,
              Impressions = x.Impressions,
              Query = x.Query
            };
        }

        public static List<KeywordStatsDto> From(List<KeywordStats> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
