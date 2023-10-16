using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class UrlWithCrawlIssuesDto
    {
        public enum ErrorCode
        {
            None = 0,
            Code301 = 1,
            Code302 = 2,
            Code4xx = 4,
            Code5xx = 8,
            BlockedByRobotsTxt = 16,
            ContainsMalware = 32,
            ImportantUrlBlockedByRobotsTxt = 64,
            DnsErrors = 128,
            TimeOutErrors = 256,
        }

        public int HttpCode;
        public long InLinks;
        public ErrorCode Issues;
        public string Url;

        public static UrlWithCrawlIssuesDto From(UrlWithCrawlIssues obj)
        {
            return new UrlWithCrawlIssuesDto()
            {
               HttpCode = obj.HttpCode,
               InLinks = obj.InLinks,
               Issues = (ErrorCode)((int)obj.Issues),
               Url = obj.Url
            };
        }

        public static List<UrlWithCrawlIssuesDto> From(List<UrlWithCrawlIssues> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
