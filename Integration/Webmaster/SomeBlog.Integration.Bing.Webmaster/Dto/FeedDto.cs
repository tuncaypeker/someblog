using BingWebmasterService;
using System.Collections.Generic;
using System.Linq;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class FeedDto
    {
        public bool Compressed;
        public long FileSize;
        public System.DateTime LastCrawled;
        public string Status;
        public System.DateTime Submitted;
        public string Type;
        public string Url;
        public int UrlCount;

        public static FeedDto From(Feed x)
        {
            return new FeedDto()
            {
                Compressed = x.Compressed,
                FileSize = x.FileSize,
                LastCrawled = x.LastCrawled,
                Status = x.Status,
                Submitted = x.Submitted,
                Type = x.Type,
                Url = x.Url,
                UrlCount = x.UrlCount
            };
        }

        public static List<FeedDto> From(List<Feed> objs)
        {
            return objs.Select(x => From(x)).ToList();
        }
    }
}
