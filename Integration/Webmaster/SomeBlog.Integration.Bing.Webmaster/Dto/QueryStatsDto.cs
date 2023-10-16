using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class QueryStatsDto
    {
        public int AvgClickPosition;
        public int AvgImpressionPosition;
        public long Clicks;
        public System.DateTime Date;
        public long Impressions;
        public string Query;

        public static QueryStatsDto From(QueryStats x)
        {
            return new QueryStatsDto()
            {
                AvgClickPosition = x.AvgClickPosition,
                AvgImpressionPosition = x.AvgImpressionPosition,
                Clicks = x.Clicks,
                Date = x.Date,
                Impressions = x.Impressions,
                Query = x.Query
            };
        }

        public static List<QueryStatsDto> From(List<QueryStats> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
