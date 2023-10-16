using BingWebmasterService;

namespace SomeBlog.Integration.Bing.Webmaster.Dto
{
    public class UrlInfoDto
    {
        public long AnchorCount;
        public System.DateTime DiscoveryDate;
        public uint DocumentSize;
        public ushort HttpStatus;
        public bool IsPage;
        public System.DateTime LastCrawledDate;
        public uint TotalChildUrlCount;
        public string Url;

        public static UrlInfoDto From(UrlInfo x)
        {
            return new UrlInfoDto()
            {
                AnchorCount = x.AnchorCount,
                DiscoveryDate = x.DiscoveryDate,
                DocumentSize = x.DocumentSize,
                HttpStatus = x.HttpStatus,
                IsPage = x.IsPage,
                LastCrawledDate = x.LastCrawledDate,
                TotalChildUrlCount = x.TotalChildUrlCount,
                Url = x.Url
            };
        }
    }
}
