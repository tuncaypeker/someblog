using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class UrlTrafficInfoDto
    {
        private long Clicks;
        private long Impressions;
        private bool IsPage;
        private string Url;

        public static UrlTrafficInfoDto From(UrlTrafficInfo x)
        {
            return new UrlTrafficInfoDto()
            {
                Clicks = x.Clicks,
                Impressions = x.Impressions,
                IsPage = x.IsPage,
                Url = x.Url
            };
        }

        public static List<UrlTrafficInfoDto> From(List<UrlTrafficInfo> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
