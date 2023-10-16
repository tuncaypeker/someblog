using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class DetailedQueryStatsDto
    {
        public long Clicks;
        public System.DateTime Date;
        public long Impressions;
        public int Position;

        public static DetailedQueryStatsDto From(DetailedQueryStats x)
        {
            return new DetailedQueryStatsDto()
            {
                Clicks = x.Clicks,
                Date = x.Date,
                Impressions = x.Impressions,
                Position = x.Position
            };
        }

        public static List<DetailedQueryStatsDto> From(List<DetailedQueryStats> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
