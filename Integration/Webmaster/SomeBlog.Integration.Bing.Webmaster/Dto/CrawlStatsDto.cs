using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class CrawlStatsDto
    {
        public long AllOtherCodes;
        public long BlockedByRobotsTxt;
        public long Code2xx;
        public long Code301;
        public long Code302;
        public long Code4xx;
        public long Code5xx;
        public long ConnectionTimeout;
        public long ContainsMalware;
        public long CrawlErrors;
        public long CrawledPages;
        public System.DateTime Date;
        public long DnsFailures;
        public long InIndex;
        public long InLinks;
        
        public static CrawlStatsDto From(CrawlStats obj)
        {
            return new CrawlStatsDto()
            {
                AllOtherCodes = obj.AllOtherCodes,
                BlockedByRobotsTxt = obj.BlockedByRobotsTxt,
                Code2xx = obj.Code2xx,
                Code301 = obj.Code301,
                Code302 = obj.Code302,
                Code4xx = obj.Code4xx,
                Code5xx = obj.Code5xx,
                ConnectionTimeout = obj.ConnectionTimeout,
                ContainsMalware = obj.ContainsMalware,
                CrawledPages = obj.CrawledPages,
                CrawlErrors = obj.CrawlErrors,
                Date = obj.Date,
                DnsFailures = obj.DnsFailures,
                InIndex =obj.InIndex,
                InLinks = obj.InLinks
            };
        }

        public static List<CrawlStatsDto> From(List<CrawlStats> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
