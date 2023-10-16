using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class RankAndTrafficStatsDto
    {
        public long Clicks;
        public System.DateTime Date;
        public long Impressions;

        public static RankAndTrafficStatsDto From(RankAndTrafficStats x)
        {
            return new RankAndTrafficStatsDto()
            {
                Clicks = x.Clicks,
                Date = x.Date,
                Impressions = x.Impressions
            };
        }

        public static List<RankAndTrafficStatsDto> From(List<RankAndTrafficStats> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
